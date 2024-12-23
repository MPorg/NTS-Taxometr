using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxometr.Data;
using static Taxometr.Services.ProviderExtentions;

namespace Taxometr.Services
{
    public class ProviderBLE
    {
        #region Init
        
        public ProviderBLE()
        {
            Initialize();
        }

        ~ProviderBLE()
        {
            try
            { 
                _charactR.ValueUpdated -= OnCharacterValueUpdated;
            }
            catch { }
        }
        public async void Initialize()
        {
            _serialNumber = await AppData.Properties.GetSerialNumber();
            _key = await AppData.Properties.GetBLEPassword();
            ReadFromBLE();
            AppData.Debug.WriteLine("ProviderBLE Initialization compleate");
        }

        #endregion

        #region Params

        private ICharacteristic _charactR = null;

        private List<byte> _answerBufer = new List<byte>();

        private FlcType _lastAnswerFlc = FlcType.NAK;
        private byte _lastFrCmd = 0x00;
        private bool _readFR = false;

        private string _serialNumber;
        private string _key;

        private int _logWidth = 60;

        #endregion

        #region BLE controll

        private async void SentToBLE(byte[] data)
        {
            try
            {
                var s = await AppData.AutoConnectDevice.GetServiceAsync(BLUETOOTH_LE_INCOTEX_SERVICE);
                var character = await s.GetCharacteristicAsync(BLUETOOTH_LE_INCOTEX_CHAR_W);
                DebugByteStr(data, "Sent data: ");
                await character.WriteAsync(data);
            }
            catch
            {
                AppData.Debug.WriteLine("Не удалось отправить сообщение");
            }
        }

        private async void ReadFromBLE()
        {
            try
            {
                var service = await AppData.AutoConnectDevice.GetServiceAsync(BLUETOOTH_LE_INCOTEX_SERVICE);
                _charactR = await service.GetCharacteristicAsync(BLUETOOTH_LE_INCOTEX_CHAR_R);
                _charactR.ValueUpdated -= OnCharacterValueUpdated;
                _charactR.ValueUpdated += OnCharacterValueUpdated;
                await _charactR.StartUpdatesAsync();
            }
            catch
            {
                AppData.Debug.WriteLine("Не удалось прочитать сообщение");
            }
        }

        private async void OnCharacterValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            if (e.Characteristic.Value != null)
            {
                //DebugByteStr(e.Characteristic.Value, $"{DateTime.Now}::{DateTime.Now.Millisecond} Reading...");
                byte[] data = e.Characteristic.Value;
                _answerBufer.AddRange(data);
                List<byte> lastBuf = new List<byte>(_answerBufer);    

                await Task.Delay(100);

                if (lastBuf.Count == _answerBufer.Count)
                {
                    //AppData.Debug.WriteLine("Reading compleate");
                    ReadFlc(_answerBufer.ToArray());
                }
            }
        }

        #endregion

        #region Debug extentions

        private void DebugLine()
        {
            string line = "";
            for (int i = 0; i < _logWidth; i++)
            {
                line += "-";
            }
            AppData.Debug.WriteLine(line);
        }

        private void DebugByteStr(byte[] data, string placeholder = "")
        {
            string dbg = placeholder;
            for (int i = 0; i < data.Length - 1; i++)
            {
                dbg += data[i].ToString("x2") + "|";
            }
            dbg += data[data.Length - 1].ToString("x2");
            AppData.Debug.WriteLine(dbg);
        }

        #endregion

        #region FLC

        public enum FlcType
        {
            REJ = 0x20,
            BUSY = 0x40,
            RST = 0x60,
            DATA = 0x80,
            ACK = 0xA0,
            RDY = 0xC0,
            NAK = 0xE0
        }

        private byte[] FlcFooter
        {
            get => new byte[5] { PREFIX, STX, ADDR, 0, 0 };
        }

        public void SentFlc(FlcType type)
        {
            DebugLine();
            AppData.Debug.WriteLine($"Отправка квитанции: {type}");

            byte[] data = FlcFooter;

            data[3] = CombineFlc((byte)type);
            data[4] = CRC7(_serialNumber, data[2], data[3]);
            SentToBLE(data);
        }

        private void ReadFlc(byte[] data)
        {
            DebugByteStr(data, "Recived data: ");

            if (data.Length >= 5)
            {
                byte reciveCrc = data[4];
                byte crc = CRC7(_serialNumber, data[2], data[3]);

                if (reciveCrc == crc)
                {
                    byte flc = data[3];
                    foreach (FlcType flcType in Enum.GetValues(typeof(FlcType)))
                    {
                        byte flcCleare = (byte)((flc >> 5) << 5);
                        if (flcCleare == (byte)flcType)
                        {
                            AppData.Debug.WriteLine($"Получена квитанция: {flcType}".FormatRight(_logWidth));
                            _lastAnswerFlc = flcType;
                            if (flcCleare == (byte)FlcType.ACK)
                            {
                                if (data.Length >= 10)
                                {
                                    List<byte> dataCut = new List<byte>(data);
                                    dataCut.RemoveRange(0, 5);
                                    DebugLine();
                                    ReadFlc(dataCut.ToArray());
                                    return;
                                }

                                if (_readFR)
                                {
                                    _readFR = false;
                                    ReadFR();
                                }
                            }
                            else if (flcCleare == (byte)FlcType.DATA)
                            {
                                ReadData(data);
                            }
                        }
                    }
                }
            }
            DebugLine();
        }

        private void ReadData(byte[] bufer)
        {
            if (bufer.Length >= 5)
            {
                List<byte> data = new List<byte>(bufer);
                data.RemoveRange(0, 6);
                byte rnd = data[2];
                
                List<byte> encodedData = new List<byte>(data);
                encodedData.RemoveRange(0, 3);
                DebugByteStr(encodedData.ToArray(), "Зашифровано: ");
                encodedData = encodedData.ToArray().RC4(_key, rnd).ToList();
                DebugByteStr(encodedData.ToArray(), "Расшифровано: ");

                List<string> answer = new List<string>();
                switch(_lastFrCmd)
                {
                    case TaxInfo:
                        answer = TaxsInfoAnsw(encodedData.ToArray());
                        break;
                    case ScnoState:
                        answer = SCNOAnsw(encodedData.ToArray());
                        break;
                }

                foreach (string a in answer)
                {
                    AppData.Debug.WriteLine(a.FormatRight(_logWidth));
                }
            }

            SentFlc(FlcType.ACK);
        }

        #endregion

        #region CMD

        public void ReadFR()
        {
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Чтение ФР\"");

            if (_lastAnswerFlc == FlcType.BUSY)
            {

            }
            else if (_lastAnswerFlc == FlcType.REJ)
            {

            }
            else
            {
                _answerBufer.Clear();
                List<byte> data = new List<byte>();
                List<byte> dataForEncode = new List<byte>();
                byte id = (byte)new Random().Next(0, 256);
                byte flag = 1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = FrRead;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                SentToBLE(CMD);
            }
        }

        public void SentScnoState(bool readFR = false)
        {
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Статус СКНО\"");

            if (_lastAnswerFlc == FlcType.BUSY)
            {

            }
            else if (_lastAnswerFlc == FlcType.REJ)
            {
                
            }
            else
            {
                _answerBufer.Clear();
                List<byte> data = new List<byte>();
                List<byte> dataForEncode = new List<byte>();
                byte id = (byte)new Random().Next(0, 256);
                byte flag = 1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = ScnoState;
                byte param = 0x01;
                byte[] crc = CRC16(_serialNumber, cmd, param);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);
                
                dataForEncode.Add(cmd);
                dataForEncode.Add(param);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastFrCmd = cmd;
                SentToBLE(CMD);
            }
        }

        public void SentTaxInfo(bool readFR = false)
        {
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Информация о таксометре\"");

            if (_lastAnswerFlc == FlcType.BUSY)
            {

            }
            else if (_lastAnswerFlc == FlcType.REJ)
            {

            }
            else
            {
                _answerBufer.Clear();
                List<byte> data = new List<byte>();
                List<byte> dataForEncode = new List<byte>();
                byte id = (byte)new Random().Next(0, 256);
                byte flag = 1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = TaxInfo;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastFrCmd = cmd;
                SentToBLE(CMD);
            }
        }

        public enum MenuMode
        {
            Main = 0,
            Drive = 1,
            Z = 2,
            X = 3
        }

        public void OpenMenuOrPrintReceipt(MenuMode menuMode, string operatorPass, bool readFR = false)
        {
            _readFR = readFR;
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Переход в режим {menuMode}\" зав. номер: {_serialNumber}, пароль оператора: {operatorPass}, пароль связи: {_key}");

            if (_lastAnswerFlc == FlcType.BUSY)
            {

            }
            else if (_lastAnswerFlc == FlcType.REJ)
            {

            }
            else
            {
                _answerBufer.Clear();

                List<byte> data = new List<byte>();
                List<byte> dataForEncode = new List<byte>();
                byte id = (byte)new Random().Next(0, 256);
                byte flag = 1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = SwitchMode;
                byte mode = (byte)menuMode;
                byte func = 0x0;
                byte[] pass = Encoding.GetEncoding(1251).GetBytes(operatorPass);

                byte[] crc = CRC16(_serialNumber, cmd, mode, func, pass[0], pass[1], pass[2], pass[3], pass[4], pass[5], 0x00, 0x00, 0x00);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.Add(mode);
                dataForEncode.Add(func);
                dataForEncode.AddRange(pass);
                byte[] blank = new byte[3] { 0x00, 0x00, 0x00 };
                dataForEncode.AddRange(blank);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                SentToBLE(CMD);
            }
        }

        public enum ButtonKey
        {
            C = 0x81,
            Up = 0x82,
            Down = 0x84,
            OK = 0x88,
            Num_1 = 0x90,
            Num_2 = 0xA0
        }

        public void EmmitButton(ButtonKey key)
        {
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Нажатие клавиши {key}\"");

            if (_lastAnswerFlc == FlcType.BUSY)
            {

            }
            else if (_lastAnswerFlc == FlcType.REJ)
            {

            }
            else
            {
                _answerBufer.Clear();

                List<byte> data = new List<byte>();
                List<byte> dataForEncode = new List<byte>();
                byte id = (byte)new Random().Next(0, 256);
                byte flag = 1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = BtnEmit;
                byte btn = (byte)key;

                byte[] crc = CRC16(_serialNumber, cmd, btn);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.Add(btn);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                SentToBLE(CMD);
            }
        }

        public void OpenShift()
        {
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Открыть смену\"");

            if (_lastAnswerFlc == FlcType.BUSY)
            {

            }
            else if (_lastAnswerFlc == FlcType.REJ)
            {

            }
            else
            {
                _answerBufer.Clear();

                List<byte> data = new List<byte>();
                List<byte> dataForEncode = new List<byte>();
                byte id = (byte)new Random().Next(0, 256);
                byte flag = 1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = ShiftOpen;

                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                SentToBLE(CMD);
            }
        }

        public enum CashMethod
        {
            Deposit = 0,
            Withdrawal = 1
        }

        public void DeposWithdrawCash(CashMethod method, ulong sum)
        {
            SentFlc(FlcType.DATA);

            string mt = "";
            switch (method)
            {
                case CashMethod.Deposit:
                    mt = "внесение";
                        break;
                case CashMethod.Withdrawal:
                    mt = "изъятие";
                    break;
            }

            AppData.Debug.WriteLine($"Отправка команды \"Служебное {mt} ({sum})\"");

            if (_lastAnswerFlc == FlcType.BUSY)
            {

            }
            else if (_lastAnswerFlc == FlcType.REJ)
            {

            }
            else
            {
                _answerBufer.Clear();

                List<byte> data = new List<byte>();
                List<byte> dataForEncode = new List<byte>();
                byte id = (byte)new Random().Next(0, 256);
                byte flag = 1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = CashDeposWithdraw;
                byte met = (byte)method;
                byte[] sumByte = BitConverter.GetBytes(sum);
                AppData.Debug.WriteLine($"sum byte length = {sumByte.Length}");

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.Add(met);
                dataForEncode.AddRange(sumByte);

                byte[] crc = CRC16(_serialNumber, dataForEncode.ToArray());

                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                SentToBLE(CMD);
            }
        }

        #endregion

        #region ANSW_Read

        private List<string> SCNOAnsw(byte[] data)
        {
            List<string> result = new List<string>();

            byte connected = data[1];

            string connectedStr = connected == 0 ? "Не подключено" : "Подключено";

            result.Add("СКНО " + connected.ToString() + " " + connectedStr);

            return result;
        }

        private List<string> TaxsInfoAnsw(byte[] data)
        {
            List<string> result = new List<string>();

            short protVer = BitConverter.ToInt16(data, 2);
            string modelName = Encoding.GetEncoding(1251).GetString(data, 4, 25);
            string apiVer = Encoding.GetEncoding(1251).GetString(data, 29, 17);
            short buildNum = BitConverter.ToInt16(data, 46);
            string buildDate = Encoding.GetEncoding(1251).GetString(data, 48, 11);
            string serialNum = Encoding.GetEncoding(1251).GetString(data, 59, 14);

            result.Add($"Версия протокола обмена: {protVer}");
            result.Add($"Имя модели: " + modelName);
            result.Add($"Версия ПО: " + apiVer);
            result.Add($"Номер сборки: {buildNum}");
            result.Add($"Дата сборки: " + buildDate);
            result.Add($"Заводской номер: " + serialNum);

            return result;  
        }

        #endregion
    }
}
