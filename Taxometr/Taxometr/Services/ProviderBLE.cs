using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Taxometr.Data;
using Xamarin.Forms;
using static Taxometr.Services.ProviderExtentions;

namespace Taxometr.Services
{
    public class ProviderBLE : IDisposable
    {
        #region Init

        public event Action<byte, Dictionary<string, string>> AnswerCompleate;

        public ProviderBLE()
        {
            Initialize();
        }

        public void Dispose()
        {
            if (_charactR != null)
            {
                _charactR.ValueUpdated -= OnCharacterValueUpdated;
                Debug.WriteLine("Provider dispose");
            }
            else
            {
                Debug.WriteLine("Provider dispose, but character been null");
            }
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
        private static int _logWidth = 60;


        private ICharacteristic _charactR = null;

        private List<byte> _answerBufer = new List<byte>();

        private FlcType _lastAnswerFlc = FlcType.NAK;
        private byte _lastCmd = 0x00;
        private byte _lastFrCmd = 0x00;
        private bool _readFR = false;

        private string _serialNumber;
        private string _key;

        #endregion

        #region BLE controll

        private int _sentToBleRetry = 0;
        private async void SentToBLE(byte[] data)
        {
            bool c = false;
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
                await Task.Delay(100);
                _sentToBleRetry++;

                if (_sentToBleRetry <= 10)
                {
                    SentToBLE(data);
                }
                else
                {
                    c = true;
                    if (await AppData.MainMenu.DisplayAlert("Не удалось отправить сообщение", "Возможно, потеряно подключение", "", "Ок"))
                    {
                        await AppData.SpecialDisconnect();
                    }
                    else
                    {
                        await AppData.SpecialDisconnect();
                    }
                    _sentToBleRetry = 0;
                    return;
                }
            }
            finally
            {
                if (c)
                    _sentToBleRetry = 0;
            }
        }

        private async void ReadFromBLE()
        {
            try
            {
                var service = await AppData.BLEAdapter.ConnectedDevices[0].GetServiceAsync(BLUETOOTH_LE_INCOTEX_SERVICE);
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

        internal static void DebugLine(bool specialDbg = false)
        {
            string line = "";
            for (int i = 0; i < _logWidth; i++)
            {
                line += "-";
            }
            AppData.Debug.WriteLine(line, specialDbg);
        }

        internal static void DebugByteStr(byte[] data, string placeholder = "", bool specialDbg = false)
        {
            string dbg = placeholder;
            for (int i = 0; i < data.Length - 1; i++)
            {
                dbg += data[i].ToString("x2") + "|";
            }
            dbg += data[data.Length - 1].ToString("x2");
            AppData.Debug.WriteLine(dbg, specialDbg);
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

        private async void ReadFlc(byte[] data)
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
                                    await Task.Delay(500);
                                    ReadFR();
                                }
                            }
                            else if (flcCleare == (byte)FlcType.DATA)
                            {
                                ReadData(data);
                            }
                            if (flcCleare == (byte)FlcType.NAK || flcCleare == (byte)FlcType.BUSY || flcCleare == (byte)FlcType.RST)
                            {
                                await Task.Delay(500);
                                Retry(true); 
                            }
                        }
                    }
                }
            }
            DebugLine();
        }

        private void RetryCMD()
        {
            RetryCMD(_lastCmd);
        }

        private async void RetryCMD(byte cmd)
        {
            await Task.Delay(100);
            switch (cmd)
            {
                case ScnoState:
                    SentScnoState(sentScnoStateReadFR, _maxRetrysCount, false);
                    break;
                case TaxInfo:
                    SentTaxInfo(sentTaxInfoReadFR, _maxRetrysCount, false);
                    break;
                case ShiftState:
                    SentShiftInfo(sentShiftInfoReadFR, _maxRetrysCount, false);
                    break;
                case SwitchMode:
                    OpenMenuOrPrintReceipt(openMenuModeMenuMode, openMenuModeOperatorPass, openMenuModeReadFR, _maxRetrysCount, false);
                    break;
                case BtnEmit:
                    EmitButton(emitButtonKey, _maxRetrysCount, false);
                    break;
                case ShiftOpen:
                    OpenShift(_maxRetrysCount, false);
                    break;
                case CashDeposWithdraw:
                    DeposWithdrawCash(deposWithdrawCashMethod, deposWithdrawSum, _maxRetrysCount, false);
                    break;
                case CheckOpen:
                    OpenCheck(openCheckStartSum, openCheckEnterSum, _maxRetrysCount, false);
                    break;
                case CheckClose:
                    CloseCheck(closeCheckCashSum, closeCheckCardSum, closeCheckNomoneySum, closeCheckTrueCashSum, _maxRetrysCount, false);
                    break;
                case TaxState:
                    SentTaxState(sentTaxStateReadFR, _maxRetrysCount, false);
                    break;
                default:
                    break;
            }
        }

        private int _retrysCount = 0;
        private int _maxRetrysCount = 15;
        private void ReadData(byte[] bufer)
        {
            Dictionary<string, string> answer = new Dictionary<string, string>();

            if (bufer.Length >= 5)
            {
                List<byte> data = new List<byte>(bufer);
                data.RemoveRange(0, 6);
                byte rnd = data[2];
                
                List<byte> encodedData = new List<byte>(data);
                encodedData.RemoveRange(0, 3);
                DebugByteStr(encodedData.ToArray(), "Зашифровано: ");
                encodedData = encodedData.RemoveDLEFlags();
                encodedData = encodedData.ToArray().RC4(_key, rnd).ToList();
                DebugByteStr(encodedData.ToArray(), "Расшифровано: ");
                
                byte cmd = encodedData[0];

                switch(cmd)
                {
                    case TaxInfo:
                        answer = TaxsInfoAnsw(encodedData.ToArray());
                        break;
                    case ScnoState:
                        answer = SCNOAnsw(encodedData.ToArray());
                        break;
                    case ShiftState:
                        answer = ShiftStateAnsw(encodedData.ToArray(), out bool retry);
                        break;
                    case TaxState:
                        answer = TaxStateAnsw(encodedData.ToArray());
                        break;
                    case CheckClose:
                        answer = CloseCheckAnsw(encodedData.ToArray());
                        break;
                    default:
                        answer = BaseAnsw(encodedData.ToArray());
                        break;
                }

                _lastFrCmd = 0;

                foreach (var a in answer)
                {
                    string answ = a.Key + a.Value;
                    AppData.Debug.WriteLine(answ.FormatRight(_logWidth));
                }
            }

            SentFlc(FlcType.ACK);

            if (answer.TryGetValue("errCode", out string value))
            {
                if (value == ENET_FR_IN_PROGRESS.ToString())
                {
                    Retry();
                }
                else if (value == ENET_WAIT_OPERATOR.ToString())
                {
                    _maxRetrysCount = -1;
                    Retry();
                }
                else
                {
                    _retrysCount = 0;
                }
            }
        }

        public async void Retry(bool retCmd = false)
        {
            if (_retrysCount >= _maxRetrysCount)
            {
                _retrysCount = 0;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (await AppData.MainMenu.DisplayAlert
                    ("Не удалось выполнить комаанду", "Пожалуйста, убедитесь, что на экране таксометра не открыто окно ввода", "<C>", "Понятно                  "))
                    {
                        byte cmd = _lastCmd;
                        int max = _maxRetrysCount;
                        EmitButton(ButtonKey.C, 1, true);
                        await Task.Delay(1000);
                        if (max >=0 ) RetryCMD(cmd);
                    }
                });
                return;
            }
            else
            {
                Debug.WriteLine($"__________________________________Retry {_retrysCount}______________________________________");
                await Task.Delay(1000);
                _retrysCount++;
                //_readFR = true;
                if(retCmd) 
                    RetryCMD();
                else
                    ReadFR();
            }
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

                //dataForEncode = dataForEncode.AddDLEFlags();
                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);
                
                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                //_lastCmd = cmd;

                SentToBLE(CMD);
            }
        }

        private bool sentScnoStateReadFR = false;
        public void SentScnoState(bool readFR = false, int retrysCount = 3)
        {
            SentScnoState(readFR, retrysCount, true);
        }

        private void SentScnoState(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;

            sentScnoStateReadFR = readFR;
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
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastFrCmd = cmd;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }

        private bool sentTaxInfoReadFR = false;
        public void SentTaxInfo(bool readFR = false, int retrysCount = 3)
        {
            SentTaxInfo(readFR, retrysCount, true);
        }
        private void SentTaxInfo(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;

            sentTaxInfoReadFR = readFR;
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
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastFrCmd = cmd;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }

        bool sentTaxStateReadFR;
        public void SentTaxState(bool readFR = false, int retrysCount = 3)
        {
            SentTaxState(readFR, retrysCount, true);
        }
        private void SentTaxState(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            sentTaxStateReadFR = readFR;
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Состояние таксометра\"");

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
                byte cmd = TaxState;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastFrCmd = cmd;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }

        public enum ShiftInfo
        {
            Closed = 0,
            Opened = 1
        }

        public ShiftInfo LastShiftState = ShiftInfo.Closed;

        private bool sentShiftInfoReadFR = false;
        public void SentShiftInfo(bool readFR = false, int retrysCount = 3)
        {
            SentShiftInfo(readFR, retrysCount, true);
        }
        private void SentShiftInfo(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            sentShiftInfoReadFR = readFR;
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Статус смены\"");

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
                byte cmd = ShiftState;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastFrCmd = cmd;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
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

        private MenuMode openMenuModeMenuMode;
        private string openMenuModeOperatorPass;
        private bool openMenuModeReadFR;
        public void OpenMenuOrPrintReceipt(MenuMode menuMode, string operatorPass, bool readFR = false, int retrysCount = 5)
        {
            OpenMenuOrPrintReceipt(menuMode, operatorPass, readFR, retrysCount, true);
        }
        private void OpenMenuOrPrintReceipt(MenuMode menuMode, string operatorPass, bool readFR = false, int retrysCount = 5, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            openMenuModeMenuMode = menuMode;
            openMenuModeOperatorPass = operatorPass;
            openMenuModeReadFR = readFR;

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
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
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

        private ButtonKey emitButtonKey;
        public void EmitButton(ButtonKey key, int retrysCount = 1)
        {
            EmitButton(key, retrysCount, true);
        }
        private void EmitButton(ButtonKey key, int retrysCount = 1, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            emitButtonKey = key;

            _readFR = true;
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
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }

        public void OpenShift(int retrysCount = 3)
        {
            OpenShift(retrysCount, true);
        }
        private void OpenShift(int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;

            _readFR = true;
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
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }

        public enum CashMethod
        {
            Deposit = 0,
            Withdrawal = 1
        }

        private CashMethod deposWithdrawCashMethod;
        private ulong deposWithdrawSum;

        public void DeposWithdrawCash(CashMethod method, ulong sum, int retrysCount = 3)
        {
            DeposWithdrawCash(method, sum, retrysCount, true);
        }
        private void DeposWithdrawCash(CashMethod method, ulong sum, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            deposWithdrawCashMethod = method;
            deposWithdrawSum = sum;

            _readFR = true;
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
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }

        int openCheckStartSum;
        int openCheckEnterSum;
        public void OpenCheck(int startSum, int enterSum, int retrysCount = 3)
        {
            OpenCheck(startSum, enterSum, retrysCount, true);
        }
        private void OpenCheck(int startSum, int enterSum, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            openCheckStartSum = startSum;
            openCheckEnterSum = enterSum;

            _readFR = true;
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Открыть чек\"");

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
                byte cmd = CheckOpen;

                byte[] startSumByte = BitConverter.GetBytes(startSum);
                byte[] enterSumByte = BitConverter.GetBytes(enterSum);

                byte[] crc = CRC16(_serialNumber, cmd,
                    startSumByte[0], startSumByte[1], startSumByte[2], startSumByte[3],
                    enterSumByte[0], enterSumByte[1], enterSumByte[2], enterSumByte[3]);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(startSumByte);
                dataForEncode.AddRange(enterSumByte);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }

        int closeCheckCashSum;
        int closeCheckCardSum;
        int closeCheckNomoneySum;
        int closeCheckTrueCashSum;
        public void CloseCheck(int cashSum, int cardSum = 0, int nomoneySum = 0, int trueCashSum = 0, int retrysCount = 10)
        {
            CloseCheck(cashSum, cardSum, nomoneySum, trueCashSum, retrysCount, true);
        }
        private void CloseCheck(int cashSum, int cardSum = 0, int nomoneySum = 0, int trueCashSum = 0, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            closeCheckCashSum = cashSum;
            closeCheckCardSum = cardSum;
            closeCheckNomoneySum = nomoneySum;
            closeCheckTrueCashSum = trueCashSum;

            int sum = cashSum + cardSum + nomoneySum;

            _readFR = true;
            SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Закрыть чек\"");

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
                byte cmd = CheckClose;

                byte[] cashSumByte = BitConverter.GetBytes(cashSum);
                byte[] cardSumByte = BitConverter.GetBytes(cardSum);
                byte[] nomoneySumByte = BitConverter.GetBytes(nomoneySum);
                byte[] sumByte = BitConverter.GetBytes(sum);
                byte[] trueCashSumByte = BitConverter.GetBytes(trueCashSum);

                byte[] crc = CRC16(_serialNumber, cmd,
                    cashSumByte[0], cashSumByte[1], cashSumByte[2], cashSumByte[3],
                    cardSumByte[0], cardSumByte[1], cardSumByte[2], cardSumByte[3],
                    nomoneySumByte[0], nomoneySumByte[1], nomoneySumByte[2], nomoneySumByte[3],
                    trueCashSumByte[0], trueCashSumByte[1], trueCashSumByte[2], trueCashSumByte[3],
                    sumByte[0], sumByte[1], sumByte[2], sumByte[3]);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(cashSumByte);
                dataForEncode.AddRange(cardSumByte);
                dataForEncode.AddRange(nomoneySumByte);
                dataForEncode.AddRange(trueCashSumByte);
                dataForEncode.AddRange(sumByte);
                dataForEncode.AddRange(crc);

                dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _lastFrCmd = cmd;
                _maxRetrysCount = retrysCount;
                SentToBLE(CMD);
            }
        }


        #endregion

        #region ANSW_Read

        private Dictionary<string,string> SCNOAnsw(byte[] data)
        {
            try
            {
                int connected = data[1];

                string connectedStr = connected == 0 ? "Не подключено" : "Подключено";

                var result = new Dictionary<string, string> { { nameof(connected), connected.ToString() }, { "Состояние скно: ", connectedStr } };
                AnswerCompleate?.Invoke(ScnoState, result);
                return result;
            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }
        }

        private Dictionary<string, string> TaxsInfoAnsw(byte[] data)
        {
            try
            {
                short protVer = BitConverter.ToInt16(data, 2);
                string modelName = Encoding.GetEncoding(1251).GetString(data, 4, 25);
                string apiVer = Encoding.GetEncoding(1251).GetString(data, 29, 17);
                short buildNum = BitConverter.ToInt16(data, 46);
                string buildDate = Encoding.GetEncoding(1251).GetString(data, 48, 11);
                string serialNum = Encoding.GetEncoding(1251).GetString(data, 59, 14);

                Dictionary<string, string> retValues = new Dictionary<string, string>
                {
                    { nameof(protVer), protVer.ToString() },
                    { nameof(modelName), modelName },
                    { nameof(apiVer), apiVer },
                    { nameof(buildNum), buildNum.ToString() },
                    { nameof(buildDate), buildDate },
                    { nameof(serialNum), serialNum }
                };

                AnswerCompleate?.Invoke(TaxInfo, retValues);
                return retValues;
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>{ { "Ошибка: ", e.Message } };
            }
        }

        private Dictionary<string, string> ShiftStateAnsw(byte[] data, out bool retry)
        {
            Debug.WriteLine("_____________________Статус смены_____________________");
            try
            {
                byte errCode = data[1];
                if (errCode != ENET_OK)
                {
                    var res1 = new Dictionary<string, string>
                    { 
                        { "Ошибка: ", ErrCodes[errCode] },
                        { "Код ошибки: ", errCode.ToString()},
                        { nameof(errCode), errCode.ToString() } 
                    };
                    AnswerCompleate?.Invoke(ShiftState, res1);

                    retry = true;
                    return res1;
                }
                int shiftState = data[2];
                LastShiftState = (ShiftInfo)shiftState;
                string res = "";

                switch (LastShiftState)
                {
                    case ShiftInfo.Opened:
                        res = "Открыта";
                        break;
                    case ShiftInfo.Closed:
                        res = "Не открыта";
                        break;
                    default:
                        res = "Не открыта";
                        break;
                }

                var result = new Dictionary<string, string>
                {
                    { nameof(errCode), errCode.ToString() },
                    { "", ErrCodes[errCode] },
                    { nameof(shiftState), shiftState.ToString() },
                    { "Состояние смены: ", res }
                };
                AnswerCompleate?.Invoke(ShiftState, result);
                retry = false;
                return result;
            }
            catch (Exception e)
            {
                retry = true;
                return new Dictionary<string, string>{ { "Ошибка: ", e.Message } };
            }
        }

        private Dictionary<string,string> TaxStateAnsw(byte[] data)
        {
            Debug.WriteLine("_____________________Статус ТАКСА_____________________");
            try
            {
                ushort errCode = BitConverter.ToUInt16(data, 2);
                int FRState = data[4];
                int menuState = data[5];
                int checkState = data[6];
                int blockState = data[7];

                string blockMessage = " ";

                if (blockState == 1)
                {
                    blockMessage = Encoding.GetEncoding(1251).GetString(data, 8, 65);
                }
                var result = new Dictionary<string, string>
                {
                    { nameof(errCode), errCode.ToString() },
                    { nameof(FRState), FRState.ToString() },
                    { nameof(menuState), menuState.ToString() },
                    { nameof(checkState), checkState.ToString() },
                    { nameof(blockState), blockState.ToString() },
                    { nameof(blockMessage), blockMessage }
                };

                AnswerCompleate?.Invoke(TaxState, result);

                return result;
            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }
        }
        private Dictionary<string, string> CloseCheckAnsw(byte[] data)
        {
            Debug.WriteLine("_____________________ Закрыть чек _____________________");
            try
            {
                byte errCode = data[1];

                if (errCode != ENET_OK)
                {
                    var result = new Dictionary<string, string>
                    {
                        { nameof(errCode), errCode.ToString() },
                        { "Ошибка: ", ErrCodes[errCode] }
                    };
                    AnswerCompleate?.Invoke(CheckClose, result);

                    return result;
                }
                else
                {
                    int surrender = BitConverter.ToInt32(data, 2);

                    var result = new Dictionary<string, string>
                    {
                        { nameof(errCode), errCode.ToString() },
                        { " ", ErrCodes[errCode] },
                        { nameof(surrender), surrender.ToString()},
                        { "Сдача: ", surrender.ToString() }
                    };

                    AnswerCompleate?.Invoke(CheckClose, result);

                    return result;
                }
            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }
        }

        private Dictionary<string, string> BaseAnsw(byte[] data)
        {
            Debug.WriteLine("_____________________ Общий ответ _____________________");
            try
            {
                byte cmd = data[0];
                byte errCode = data[1];
                if (errCode != ENET_OK)
                {
                    var result = new Dictionary<string, string>
                    {
                        { nameof(errCode), errCode.ToString() },
                        { "Ошибка: ", ErrCodes[errCode] }
                    };
                    AnswerCompleate?.Invoke(cmd, result);

                    return result;
                }
                else
                {
                    var result = new Dictionary<string, string>
                    {
                        { nameof(errCode), errCode.ToString() },
                        { "Ответ: ", ErrCodes[errCode] },
                        { "cmd", cmd.ToString()}
                    };
                    AnswerCompleate?.Invoke(cmd, result);

                    return result;
                }
            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }
        }

        #endregion
    }
}
