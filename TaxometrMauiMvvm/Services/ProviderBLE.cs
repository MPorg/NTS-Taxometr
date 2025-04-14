using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using TaxometrMauiMvvm.Data;
using static TaxometrMauiMvvm.Services.ProviderExtentions;

namespace TaxometrMauiMvvm.Services
{

    public class ProviderBLE : IDisposable
    {

        #region Params

        public void GetErr(byte cmd, Dictionary<string, string> answer)
        {
            ErrMessageReaded?.Invoke(cmd, answer);
        }

        public event Action<byte, Dictionary<string, string>>? ErrMessageReaded;

        private ProviderState _state = ProviderState.Idle;

        public event Action<byte, Dictionary<string, string>>? AnswerCompleate;
        private readonly ConcurrentQueue<Action> _cmdQueue = new ConcurrentQueue<Action>();
        private bool _next = true;

        private readonly static int _logWidth = 60;

        private ICharacteristic? _charactR = null;

        private readonly List<byte> _answerBufer = [];

        private FlcType _lastAnswerFlc = FlcType.NAK;
        private byte _lastCmd = 0x00;
        private bool _readFR = false;

        private byte _lastCmdId = 0x00;
        private byte _currentCmdId = 0x00;

        private string? _serialNumber;
        private string? _key;

        private int _sentToBleRetry = 0;

        private readonly Timer _statesTimer = new Timer(0);

        #endregion

        #region Init

        public ProviderBLE()
        {
        }

        public void Dispose()
        {
            if (_charactR != null)
            {
                _charactR.ValueUpdated -= OnCharacterValueUpdated;
                //await _charactR.StopUpdatesAsync();
            }
        }

        private async Task<bool> TrySetStateAndDo(ProviderState newState, Task<bool> delegateBool)
        {
            Debug.WriteLine($"_________________________ {_statesTimer.CurrentTime} - {_state} => {newState}  ________________________________");

            if (newState == ProviderState.SentFLC_2)
            {
                if (await delegateBool)
                {
                    Debug.WriteLine("-----------------Timer stop, next = true----------------");
                    _statesTimer.Stop();
                    _state = ProviderState.Idle;
                    _extreamCleare = false;
                    _next = true;
                    return true;
                }
                return false;
            }
            else
            {
                if (await delegateBool)
                {
                    Debug.WriteLine("-----------------Timer restert 3000ms----------------");
                    _statesTimer.SetMaxMillis(1500);
                    _statesTimer.Restart();
                    _state = newState;
                    _extreamCleare = false;
                    return true;
                }
                return false;
            }


            /*if ((_state == ProviderState.Idle || _state == ProviderState.SentFLC_1 || _state == ProviderState.ReciveFLC_0) && newState == ProviderState.SentFLC_0)
            {
                if (await delegateBool)
                {
                    _statesTimer.SetMaxMillis(500);
                    _statesTimer.Restart();
                    _state = newState;
                    _extreamCleare = false;
                    return true;
                }
                return false;
            }
            else if (_state == ProviderState.SentFLC_0 && newState == ProviderState.SentData_0)
            {
                if (await delegateBool)
                {
                    _statesTimer.SetMaxMillis(3000);
                    _statesTimer.Restart();
                    _state = newState;
                    _extreamCleare = false;
                    return true;
                }
                return false;
            }
            else if ((_state == ProviderState.SentData_0 || _state == ProviderState.SentFR) && newState == ProviderState.ReciveFLC_0)
            {
                if (await delegateBool)
                {
                    _statesTimer.SetMaxMillis(3000);
                    _statesTimer.Restart();
                    _state = newState;
                    _extreamCleare = false;
                    return true;
                }
                return false;
            }
            else if ((_state == ProviderState.ReciveFLC_0 || _state == ProviderState.Idle || _state == ProviderState.SentFLC_0) && newState == ProviderState.SentFLC_1)
            {
                if (await delegateBool)
                {
                    _statesTimer.SetMaxMillis(500);
                    _statesTimer.Restart();
                    _state = newState;
                    _extreamCleare = false;
                    return true;
                }
                return false;
            }
            else if (_state == ProviderState.SentFLC_1 && newState == ProviderState.SentFR)
            {
                if (await delegateBool)
                {
                    _statesTimer.SetMaxMillis(3000);
                    _statesTimer.Restart();
                    _state = newState;
                    _extreamCleare = false;
                    return true;
                }
                return false;
            }
            else if (_state == ProviderState.ReciveFLC_0 && newState == ProviderState.ReciveFLC_1)
            {
                if (await delegateBool)
                {
                    _statesTimer.SetMaxMillis(500);
                    _statesTimer.Restart();
                    _state = newState;
                    _extreamCleare = false;
                    return true;
                }
                return false;
            }
            else if ((_state == ProviderState.ReciveFLC_1 || _state == ProviderState.ReciveFLC_0) && newState == ProviderState.SentFLC_2)
            {
                if (await delegateBool)
                {
                    _statesTimer.Stop();
                    _state = ProviderState.Idle;
                    _extreamCleare = false;
                    _next = true;
                    return true;
                }
                return false;
            }
            return false;*/
        }

        private bool TrySetStateAndDo(ProviderState newState, Action? action)
        {
            Debug.WriteLine($"_________________________ {_statesTimer.CurrentTime} - {_state} => {newState}  ________________________________");
            if ((_state == ProviderState.Idle || _state == ProviderState.SentFLC_1 || _state == ProviderState.ReciveFLC_0) && newState == ProviderState.SentFLC_0)
            {
                _statesTimer.SetMaxMillis(500);
                _statesTimer.Restart();
                _state = newState;
                _extreamCleare = false;
                _retryStop = false;
                action?.Invoke();
                return true;
            }
            else if (_state == ProviderState.SentFLC_0 && newState == ProviderState.SentData_0)
            {
                _statesTimer.SetMaxMillis(3000);
                _statesTimer.Restart();
                _state = newState;
                _extreamCleare = false;
                _retryStop = false;
                action?.Invoke();
                return true;
            }
            else if ((_state == ProviderState.SentData_0 || _state == ProviderState.SentFR) && newState == ProviderState.ReciveFLC_0)
            {
                _statesTimer.SetMaxMillis(3000);
                _statesTimer.Restart();
                _state = newState;
                _extreamCleare = false;
                _retryStop = false;
                action?.Invoke();
                return true;
            }
            else if ((_state == ProviderState.ReciveFLC_0 || _state == ProviderState.Idle || _state == ProviderState.SentFLC_0) && newState == ProviderState.SentFLC_1)
            {
                _statesTimer.SetMaxMillis(500);
                _statesTimer.Restart();
                _state = newState;
                _extreamCleare = false;
                _retryStop = false;
                action?.Invoke();
                return true;
            }
            else if (_state == ProviderState.SentFLC_1 && newState == ProviderState.SentFR)
            {
                _statesTimer.SetMaxMillis(3000);
                _statesTimer.Restart();
                _state = newState;
                _extreamCleare = false;
                _retryStop = false;
                action?.Invoke();
                return true;
            }
            else if (_state == ProviderState.ReciveFLC_0 && newState == ProviderState.ReciveFLC_1)
            {
                _statesTimer.SetMaxMillis(500);
                _statesTimer.Restart();
                _state = newState;
                _extreamCleare = false;
                _retryStop = false;
                action?.Invoke();
                return true;
            }
            else if ((_state == ProviderState.ReciveFLC_1 || _state == ProviderState.ReciveFLC_0) && newState == ProviderState.SentFLC_2)
            {
                _statesTimer.Stop();
                _state = ProviderState.Idle;
                _extreamCleare = false;
                _retryStop = false;
                action?.Invoke();
                return true;
            }
            return false;
        }

        bool _extreamCleare = true;
        public async Task Initialize()
        {
            _serialNumber = await AppData.Properties.GetSerialNumber();
            _key = await AppData.Properties.GetBLEPassword();
            _statesTimer.OnTimeout += StatesTimer_OnTimeout;
            await ReadFromBLE();
            AppData.Debug.WriteLine("ProviderBLE Initialization compleate");
            Application.Current?.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(250), new Func<bool>(() =>
            {
                if (_next && _state == ProviderState.Idle)
                {
                    if (_cmdQueue.TryDequeue(out Action? result))
                    {
                        _next = false;
                        Application.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(5), new Func<bool>(() =>
                        {
                            bool res = !_extreamCleare;
                            if (!res)
                            {
                                _retryStop = true;
                            }
                            else
                            {
                                _extreamCleare = true;
                            }
                            return res;
                        }));
                        result?.Invoke();
                    }
                }
                return true;
            }));
        }

        private async void StatesTimer_OnTimeout()
        {
            Debug.WriteLine($"_________________________ {_statesTimer.CurrentTime} - {_state} => Timeout  ________________________________");
            var state = _state;
            _state = ProviderState.Idle;
            /*if (_retryStop)
            {
                if (state == ProviderState.SentFR || state == ProviderState.ReciveFLC_1)
                {
                    await ReadFR();
                }
                else
                {
                    *//*Debug.WriteLine($"Extreem Clear {_cmdQueue.Count}");
                    _cmdQueue.Clear();
                    _next = true;*//*
                }
                return;
            }*/
            await ReadFR();
        }

        #endregion

        #region BLE controll

        private async Task<bool> SentToBLE(byte[] data)
        {
            bool c = false;
            try
            {
                var s = await AppData.ConnectedDevice.GetServiceAsync(BLUETOOTH_LE_INCOTEX_SERVICE);
                var character = await s.GetCharacteristicAsync(BLUETOOTH_LE_INCOTEX_CHAR_W);
                DebugByteStr(data, "Sent data: ");
                await character.WriteAsync(data);
                return true;
            }
            catch
            {
                c = await SentToBLERetry(data);
            }
            finally
            {
                if (c)
                    _sentToBleRetry = 0;

            }
            return false;
        }

        private bool _retryStop = false;

        private async Task<bool> SentToBLERetry(byte[] data)
        {
            AppData.Debug.WriteLine("Не удалось отправить сообщение");
            await Task.Delay(100);
            _sentToBleRetry++;

            if (_sentToBleRetry <= 10)
            {
                await SentToBLE(data);
                return false;
            }
            else
            {
                bool c = true;
                _retryStop = true;
                if (AppData.MainMenu != null && await AppData.MainMenu.DisplayAlert("Не удалось отправить сообщение", "Возможно, потеряно подключение", "", "Ок"))
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await AppData.SpecialDisconnect();
                    });
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await AppData.SpecialDisconnect();
                    });
                }
                _sentToBleRetry = 0;
                return c;
            }
        }

        private async Task ReadFromBLE()
        {
            try
            {
                if (_charactR != null) _charactR.ValueUpdated -= OnCharacterValueUpdated;

                var service = await AppData.BLEAdapter.ConnectedDevices[0].GetServiceAsync(BLUETOOTH_LE_INCOTEX_SERVICE);
                _charactR = await service.GetCharacteristicAsync(BLUETOOTH_LE_INCOTEX_CHAR_R);

                _charactR.ValueUpdated += OnCharacterValueUpdated;

                await _charactR.StartUpdatesAsync();
            }
            catch
            {
                AppData.Debug.WriteLine("Не удалось прочитать сообщение");
            }
        }

        private async void OnCharacterValueUpdated(object? sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            try
            {
                if (e.Characteristic.Value != null)
                {
                    //DebugByteStr(e.Characteristic.Value, $"{DateTime.Now}::{DateTime.Now.Millisecond} Reading...");
                    byte[] data = e.Characteristic.Value;
                    if (data != null && data.Length > 0)
                    {
                        _answerBufer.AddRange(data);
                        List<byte> lastBuf = new List<byte>(_answerBufer);    

                        await Task.Delay(100);

                        if (lastBuf.Count == _answerBufer.Count)
                        {
                            //AppData.Debug.WriteLine("Reading compleate");
                            await ReadFlc(_answerBufer.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.ShowToast($"Ошибка в ProviderBLE: {ex.Message}");
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
            try
            {
                string dbg = placeholder;
                for (int i = 0; i < data.Length - 1; i++)
                {
                    dbg += data[i].ToString("x2") + "|";
                }
                dbg += data[data.Length - 1].ToString("x2");
                AppData.Debug.WriteLine(dbg, specialDbg);
            }
            catch (Exception e)
            {
                AppData.Debug.WriteLine(e.Message.ToString());
            }
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

        public async Task SentFlc(FlcType type, bool fromFr = false, bool lastFLC = false)
        {
            DebugLine();
            AppData.Debug.WriteLine($"Отправка квитанции: {type}");

            byte[] data = FlcFooter;

            data[3] = CombineFlc((byte)type);

            if (_serialNumber == null) return;
            data[4] = CRC7(_serialNumber, data[2], data[3]);

            await TrySetStateAndDo(lastFLC ? ProviderState.SentFLC_2 : fromFr ? ProviderState.SentFLC_1 : ProviderState.SentFLC_0, SentToBLE(data));
        }

        private async Task ReadFlc(byte[] data)
        {
            try
            {
                DebugByteStr(data, "Recived data: ");

                if (data.Length >= 5)
                {
                    byte reciveCrc = data[4];

                    if (_serialNumber == null) return;
                    byte crc = CRC7(_serialNumber, data[2], data[3]);

                    if (reciveCrc == crc)
                    {
                        byte flc = data[3];
                        foreach (FlcType flcType in Enum.GetValues<FlcType>())
                        {
                            byte flcCleare = (byte)((flc >> 5) << 5);
                            if (flcCleare == (byte)flcType)
                            {
                                AppData.Debug.WriteLine($"Получена квитанция: {flcType}".FormatRight(_logWidth));
                                _lastAnswerFlc = flcType;
                                if (flcCleare == (byte)FlcType.ACK)
                                {
                                    TrySetStateAndDo(ProviderState.ReciveFLC_0, () => RecivedAck(data));
                                }
                                else if (flcCleare == (byte)FlcType.DATA)
                                {
                                    if (await TrySetStateAndDo(ProviderState.ReciveFLC_1, ReadDataAsync(data)))
                                    {
                                        await SentFlc(FlcType.ACK, false, true);
                                    }
                                }
                                if (flcCleare == (byte)FlcType.NAK || flcCleare == (byte)FlcType.BUSY || flcCleare == (byte)FlcType.RST)
                                {
                                    await ReadFR();
                                }
                            }
                        }
                    }
                }
                DebugLine();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"______________Ошибка: {ex.Message}________________"); 
                if ((int)_state >= (int)ProviderState.SentFR || (int)_state == (int)ProviderState.ReciveFLC_0)
                {
                    _state = ProviderState.ReciveFLC_0;
                    _statesTimer.SetMaxMillis(500);
                    _statesTimer.Restart();
                    _extreamCleare = false;
                    _retryStop = false;
                    await ReadFR();
                }
            }
        }

        private async void RecivedAck(byte[] data)
        {
            if (data.Length >= 10)
            {
                List<byte> dataCut = new List<byte>(data);
                dataCut.RemoveRange(0, 5);
                DebugLine();
                await ReadFlc(dataCut.ToArray());
                return;
            }

            if (_readFR)
            {
                _readFR = false;
                await Task.Delay(500);
                await ReadFR();
                return;
            }
            else
            {
                await SentFlc(FlcType.ACK, false, true);
                return;
            }
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
                    await SentScnoState(sentScnoStateReadFR, _maxRetrysCount, false);
                    break;
                case TaxInfo:
                    await SentTaxInfo(sentTaxInfoReadFR, _maxRetrysCount, false);
                    break;
                case ShiftState:
                    await SentShiftInfo(sentShiftInfoReadFR, _maxRetrysCount, false);
                    break;
                case SwitchMode:
                    if (openMenuModeOperatorPass != null)
                        await OpenMenuOrPrintReceipt(openMenuModeMenuMode, openMenuModeOperatorPass, openMenuModeReadFR, _maxRetrysCount, false);
                    break;
                case BtnEmit:
                    await EmitButton(emitButtonKey, _maxRetrysCount, false);
                    break;
                case ShiftOpen:
                    await OpenShift(_openShiftSilentMode, _maxRetrysCount, false);
                    break;
                case CashDeposWithdraw:
                    await DeposWithdrawCash(deposWithdrawCashMethod, deposWithdrawSum, _maxRetrysCount, false);
                    break;
                case CheckState:
                    await SentCheckState(sentCheckStateReadFR, _maxRetrysCount, false);
                    break;
                case CheckOpen:
                    await OpenCheck(openCheckStartSum, openCheckEnterSum, _maxRetrysCount, false);
                    break;
                case CheckClose:
                    await CloseCheck(closeCheckCashSum, closeCheckCardSum, closeCheckNomoneySum, closeCheckTrueCashSum, _maxRetrysCount, false);
                    break;
                case CheckBreak:
                    await BreakCheck(_maxRetrysCount, false);
                    break;
                case CheckDuplicate:
                    await CopyCheck(_maxRetrysCount, false);
                    break;
                case TaxState:
                    await SentTaxState(sentTaxStateReadFR, _maxRetrysCount, false);
                    break;
                default:
                    break;
            }
        }

        private float _retrysCount = 0;
        private int _maxRetrysCount = 15;

        private async Task<bool> ReadDataAsync(byte[] bufer)
        {
            try
            {
                Dictionary<string, string> answer = new Dictionary<string, string>();

                if (bufer.Length >= 5)
                {
                    List<byte> data = new List<byte>(bufer);
                    data.RemoveRange(0, 6);
                    _currentCmdId = data[0];

                    DebugByteStr(new byte[2] {_lastCmdId,  _currentCmdId}, placeholder: "____________________________________Отправлен пакет | получен пакет:");

                    byte rnd = data[2];

                    List<byte> dataForEncode = new List<byte>(data);
                    dataForEncode.RemoveRange(0, 3);
                    DebugByteStr(dataForEncode.ToArray(), "Зашифровано: ");
                    dataForEncode = dataForEncode.RemoveDLEFlags();
                    if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                    DebugByteStr(dataForEncode.ToArray(), "Расшифровано: ");

                    byte cmd = dataForEncode[0];

                    answer = _lastCmd switch
                    {
                        TaxInfo => TaxsInfoAnsw(dataForEncode.ToArray()),
                        ScnoState => SCNOAnsw(dataForEncode.ToArray()),
                        ShiftState => ShiftStateAnsw(dataForEncode.ToArray()),
                        TaxState => TaxStateAnsw(dataForEncode.ToArray()),
                        CheckState => CheckStateAnsw(dataForEncode.ToArray()),
                        CheckClose => CloseCheckAnsw(dataForEncode.ToArray()),
                        _ => BaseAnsw(dataForEncode.ToArray()),
                    };
                    foreach (var a in answer)
                    {
                        string answ = a.Key + (a.Key.Contains(':') ? "" : ": ") + a.Value;
                        AppData.Debug.WriteLine(answ.FormatRight(_logWidth));
                    }
                }

                if (answer.TryGetValue("err", out string? errVal))
                {
                    if (errVal == "cmdId")
                    {
                        return true;
                    }
                }


                if (answer.TryGetValue("errCode", out string? value))
                {
                    if (value == ENET_FR_IN_PROGRESS.ToString("x2"))
                    {
                        _cmdQueue.Enqueue(async () => { await ReadFR(); });
                    }
                    else if (value == ENET_WAIT_OPERATOR.ToString("x2"))
                    {
                        _cmdQueue.Enqueue(async () => 
                        {
                            _maxRetrysCount = -1;
                            if (_lastCmd != ShiftOpen)
                            {
                                MainThread.BeginInvokeOnMainThread(async () =>
                                {
                                    if (AppData.MainMenu != null && await AppData.MainMenu.DisplayAlert
                                    ("Не удалось выполнить комаанду", "Пожалуйста, убедитесь, что на экране таксометра не открыто окно ввода", "<C>", "Понятно                  "))
                                    {
                                        byte cmd = _lastCmd;
                                        int max = _maxRetrysCount;
                                        await EmitButton(ButtonKey.C, 1, true);
                                        await Task.Delay(500);
                                        //if (max >= 0) RetryCMD(cmd);
                                    }
                                });
                            }
                            else
                            {
                                _next = true;
                                AnswerCompleate?.Invoke(_lastCmd, answer);
                            }
                        });
                    }
                    else
                    {
                        _retrysCount = 0;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"______________Ошибка: {ex.Message}________________");
                if ((int)_state >= (int)ProviderState.SentFR || (int)_state == (int)ProviderState.ReciveFLC_0)
                {
                    _state = ProviderState.ReciveFLC_0;
                    _statesTimer.SetMaxMillis(500);
                    _statesTimer.Restart();
                    _extreamCleare = false;
                    _retryStop = false;
                    await ReadFR();
                    return true;
                }
                return false;
            }  
            finally
            {
                _next = true;
            }
        }

        #endregion

        #region CMD

        public async Task ReadFR()
        {
            await SentFlc(FlcType.ACK);
            await SentFlc(FlcType.DATA, true);

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
                byte id = _lastCmdId;
                //_lastCmdId = id; DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = FrRead;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                //dataForEncode = dataForEncode.AddDLEFlags();
                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);
                
                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                //_lastCmd = cmd;

                await TrySetStateAndDo(ProviderState.SentFR, SentToBLE(CMD));
            }
        }

        private bool sentScnoStateReadFR = false;
        public void SentScnoState(bool readFR = false, int retrysCount = 3)
        {
            _cmdQueue.Enqueue(new Action(async () => { await SentScnoState(readFR, retrysCount, true); }));
        }

        private async Task SentScnoState(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;

            sentScnoStateReadFR = readFR;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = ScnoState;
                byte param = 0x01;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd, param);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);
                
                dataForEncode.Add(cmd);
                dataForEncode.Add(param);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        private bool sentTaxInfoReadFR = false;
        public void SentTaxInfo(bool readFR = false, int retrysCount = 3)
        {
            _cmdQueue.Enqueue(new Action(async () => { await SentTaxInfo(readFR, retrysCount, true); }));
        }
        private async Task SentTaxInfo(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;

            sentTaxInfoReadFR = readFR;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = TaxInfo;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        bool sentTaxStateReadFR;
        public void SentTaxState(bool readFR = false, int retrysCount = 3)
        {
            _cmdQueue.Enqueue(new Action(async () => { await SentTaxState(readFR, retrysCount, true); }));
        }
        private async Task SentTaxState(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            sentTaxStateReadFR = readFR;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = TaxState;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
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
            _cmdQueue.Enqueue(new Action(async () => { await SentShiftInfo(readFR, retrysCount, true); }));
            
        }
        private async Task SentShiftInfo(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            sentShiftInfoReadFR = readFR;
            await SentFlc(FlcType.DATA);

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
                //byte id = (byte)new Random().Next(0, 256);
                byte id = 0x00;
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = ShiftState;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = [.. data, .. dataForEncode];

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
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
        private string? openMenuModeOperatorPass;
        private bool openMenuModeReadFR;
        public void OpenMenuOrPrintReceipt(MenuMode menuMode, string operatorPass, bool readFR = true, int retrysCount = 5)
        {
            _cmdQueue.Enqueue(new Action(async () => { await OpenMenuOrPrintReceipt(menuMode, operatorPass, readFR, retrysCount, true); }));
            
        }
        private async Task OpenMenuOrPrintReceipt(MenuMode menuMode, string operatorPass, bool readFR = true, int retrysCount = 5, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            openMenuModeMenuMode = menuMode;
            openMenuModeOperatorPass = operatorPass;
            openMenuModeReadFR = readFR;

            _readFR = readFR;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = SwitchMode;
                byte mode = (byte)menuMode;
                byte func = 0x00;
                byte[] pass = Encoding.GetEncoding(1251).GetBytes(operatorPass);


                if (_serialNumber == null) return;
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

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        public enum ButtonKey
        {
            None = 0x00,

            C = 0x81,
            Up = 0x82,
            Down = 0x84,
            OK = 0x88,
            Num_1 = 0xA0,
            Num_2 = 0x90,

            Any = C | Up | Down | OK | Num_1 | Num_2 
        }

        private ButtonKey emitButtonKey;
        public void EmitButton(ButtonKey key, int retrysCount = 1)
        {
            _cmdQueue.Enqueue(new Action(async () => { await EmitButton(key, retrysCount, true); }));
        }
        private async Task EmitButton(ButtonKey key, int retrysCount = 1, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            emitButtonKey = key;

            _readFR = true;
            await SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Нажатие клавиши {key} (0x{((byte)key).ToString("x2")})\"");

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = BtnEmit;
                byte btn = (byte)key;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd, btn);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.Add(btn);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        private bool _openShiftSilentMode = false;

        public void OpenShift(bool silentMode = false, int retrysCount = 3)
        {
            _cmdQueue.Enqueue(new Action(async () => { await OpenShift(silentMode, retrysCount, true); }));
            
        }
        private async Task OpenShift(bool silentMode = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            _openShiftSilentMode = silentMode;
            _readFR = true;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = ShiftOpen;
                byte silent = silentMode == true ? (byte)0 : (byte)1;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd, silent);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.Add(silent);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
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
            _cmdQueue.Enqueue(new Action(async () => {await DeposWithdrawCash(method, sum, retrysCount, true); }));
        }
        private async Task DeposWithdrawCash(CashMethod method, ulong sum, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            deposWithdrawCashMethod = method;
            deposWithdrawSum = sum;

            _readFR = true;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id;
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
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

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, dataForEncode.ToArray());

                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        private bool sentCheckStateReadFR = false;
        public void SentCheckState(bool readFR = false, int retrysCount = 3)
        {
            _cmdQueue.Enqueue(new Action(async () => { await SentCheckState(readFR, retrysCount, true); }));

        }
        private async Task SentCheckState(bool readFR = false, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            sentCheckStateReadFR = readFR;
            await SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Статус чека\"");

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = CheckState;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _readFR = readFR;
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        int openCheckStartSum;
        int openCheckEnterSum;
        public void OpenCheck(int startSum, int enterSum, int retrysCount = 3)
        {
            _cmdQueue.Enqueue(new Action(async () => { await OpenCheck(startSum, enterSum, retrysCount, true); }));
        }
        private async Task OpenCheck(int startSum, int enterSum, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            openCheckStartSum = startSum;
            openCheckEnterSum = enterSum;

            _readFR = true;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = CheckOpen;

                byte[] startSumByte = BitConverter.GetBytes(startSum);
                byte[] enterSumByte = BitConverter.GetBytes(enterSum);


                if (_serialNumber == null) return;
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

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        int closeCheckCashSum;
        int closeCheckCardSum;
        int closeCheckNomoneySum;
        int closeCheckTrueCashSum;
        public void CloseCheck(int cashSum, int cardSum = 0, int nomoneySum = 0, int trueCashSum = 0, int retrysCount = 10)
        {
            _cmdQueue.Enqueue(new Action(async () => { await CloseCheck(cashSum, cardSum, nomoneySum, trueCashSum, retrysCount, true); }));
        }
        private async Task CloseCheck(int cashSum, int cardSum = 0, int nomoneySum = 0, int trueCashSum = 0, int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;
            closeCheckCashSum = cashSum;
            closeCheckCardSum = cardSum;
            closeCheckNomoneySum = nomoneySum;
            closeCheckTrueCashSum = trueCashSum;

            int sum = cashSum + cardSum + nomoneySum;

            _readFR = true;
            await SentFlc(FlcType.DATA);

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = CheckClose;

                byte[] cashSumByte = BitConverter.GetBytes(cashSum);
                byte[] cardSumByte = BitConverter.GetBytes(cardSum);
                byte[] nomoneySumByte = BitConverter.GetBytes(nomoneySum);
                byte[] sumByte = BitConverter.GetBytes(sum);
                byte[] trueCashSumByte = BitConverter.GetBytes(trueCashSum);


                if (_serialNumber == null) return;
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

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }
        public void BreakCheck(int retrysCount = 10)
        {
            _cmdQueue.Enqueue(new Action(async () => { await BreakCheck(retrysCount, true); }));
        }
        private async Task BreakCheck(int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;

            _readFR = true;
            await SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Отменить чек\"");

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
                _lastCmdId = id;
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = CheckBreak;

                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        public void CopyCheck(int retrysCount = 10)
        {
            _cmdQueue.Enqueue(new Action(async () => { await CopyCheck(retrysCount, true); }));
        }
        private async Task CopyCheck(int retrysCount = 3, bool firstTry = false)
        {
            if (firstTry) _retrysCount = 0;

            _readFR = true;
            await SentFlc(FlcType.DATA);

            AppData.Debug.WriteLine($"Отправка команды \"Отменить чек\"");

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
                _lastCmdId = id; 
                DebugByteStr(new byte[1]{id}, "______________________________________________________ID: ");
                byte flag = _key == "000000" ? (byte)0 : (byte)1;
                byte rnd = (byte)new Random().Next(0, 256);
                byte cmd = CheckDuplicate;


                if (_serialNumber == null) return;
                byte[] crc = CRC16(_serialNumber, cmd);

                data.Add(id);
                data.Add(flag);
                data.Add(rnd);

                dataForEncode.Add(cmd);
                dataForEncode.AddRange(crc);

                if (_key != null && _key != "000000") dataForEncode = dataForEncode.ToArray().RC4(_key, rnd).ToList();
                //dataForEncode = dataForEncode.AddDLEFlags();

                List<byte> result = new List<byte>();
                result.AddRange(data);
                result.AddRange(dataForEncode);

                byte[] CMD = CombineData(result.ToArray(), _serialNumber);
                _lastCmd = cmd;
                _maxRetrysCount = retrysCount;

                await TrySetStateAndDo(ProviderState.SentData_0, SentToBLE(CMD));
            }
        }

        #endregion

        #region ANSW_Read

        private Dictionary<string,string> SCNOAnsw(byte[] data)
        {
            var result = BaseAnsw(data, false);

            if (result.TryGetValue("err", out string? value))
            {
                if (value == "cmdId")
                {
                    return result;
                }
            }

            try
            {
                if (result.TryGetValue("errCode", out string? errCode))
                {
                    if (errCode != "00")
                    {
                        AnswerCompleate?.Invoke(ScnoState, result);

                        return result;
                    }
                    else
                    {
                        int connected = data[1];

                        string connectedStr = connected == 0 ? "Не подключено" : "Подключено";

                        result.Add(nameof(connected), connected.ToString());
                        result.Add("Состояние скно: ", connectedStr);

                        AnswerCompleate?.Invoke(ScnoState, result);
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }

            return result;
        }

        private Dictionary<string, string> TaxsInfoAnsw(byte[] data)
        {
            var result = BaseAnsw(data, false);

            if (result.TryGetValue("err", out string? value))
            {
                if (value == "cmdId")
                {
                    return result;
                }
            }

            try
            {
                if (result.TryGetValue("errCode", out string? errCode))
                {
                    if (errCode != "00")
                    {
                        AnswerCompleate?.Invoke(TaxState, result);

                        return result;
                    }
                    else
                    {
                        short protVer = BitConverter.ToInt16(data, 2);
                        string modelName = Encoding.GetEncoding(1251).GetString(data, 4, 25);
                        string apiVer = Encoding.GetEncoding(1251).GetString(data, 29, 17);
                        short buildNum = BitConverter.ToInt16(data, 46);
                        string buildDate = Encoding.GetEncoding(1251).GetString(data, 48, 11);
                        string serialNum = Encoding.GetEncoding(1251).GetString(data, 59, 14);

                        result.Add( nameof(protVer), protVer.ToString() );
                        result.Add( nameof(modelName), modelName );
                        result.Add( nameof(apiVer), apiVer );
                        result.Add( nameof(buildNum), buildNum.ToString() );
                        result.Add( nameof(buildDate), buildDate );
                        result.Add( nameof(serialNum), serialNum );

                        AnswerCompleate?.Invoke(TaxInfo, result);
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }

            return result;
        }

        private Dictionary<string, string> ShiftStateAnsw(byte[] data)
        {
            var result = BaseAnsw(data, false);

            if (result.TryGetValue("err", out string? value))
            {
                if (value == "cmdId")
                {
                    return result;
                }
            }

            try
            {
                if (result.TryGetValue("errCode", out string? errCode))
                {
                    if (errCode != "00")
                    {
                        AnswerCompleate?.Invoke(CheckClose, result);

                        return result;
                    }
                    else
                    {

                        int shiftState = data[2];
                        LastShiftState = (ShiftInfo)shiftState;
                        string res = "";

                        res = LastShiftState switch
                        {
                            ShiftInfo.Opened => "Открыта",
                            ShiftInfo.Closed => "Не открыта",
                            _ => "Не открыта",
                        };
                        result.Add(nameof(shiftState), shiftState.ToString());
                        result.Add("Состояние смены: ", res);

                        AnswerCompleate?.Invoke(ShiftState, result);
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                return new Dictionary<string, string>{ { "Ошибка: ", e.Message } };
            }

            return result;
        }

        private Dictionary<string, string> TaxStateAnsw(byte[] data)
        {
            var result = BaseAnsw(data, false);

            if (result.TryGetValue("err", out string? value))
            {
                if (value == "cmdId")
                {
                    return result;
                }
            }

            try
            {
                if (result.TryGetValue("errCode", out string? errCode))
                {
                    if (errCode != "00")
                    {
                        AnswerCompleate?.Invoke(TaxState, result);

                        return result;
                    }
                    else
                    {
                        int FRState = data[4];
                        int menuState = data[5];
                        int checkState = data[6];
                        int blockState = data[7];

                        string blockMessage = "";

                        if (blockState != 0)
                        {
                            blockMessage = Encoding.GetEncoding(1251).GetString(data, 8, 65);
                        }

                        result.Add(nameof(FRState), FRState.ToString());
                        result.Add(nameof(menuState), menuState.ToString());
                        result.Add(nameof(checkState), checkState.ToString());
                        result.Add(nameof(blockState), blockState.ToString());
                        if (!string.IsNullOrEmpty(blockMessage)) result.Add(nameof(blockMessage), blockMessage);

                        AnswerCompleate?.Invoke(TaxState, result);

                        return result;
                    }
                }

            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }

            return result;
        }

        private Dictionary<string, string> CheckStateAnsw(byte[] data)
        {
            var result = BaseAnsw(data, false);

            if (result.TryGetValue("err", out string? value))
            {
                if (value == "cmdId")
                {
                    return result;
                }
            }

            try
            {
                if (result.TryGetValue("errCode", out string? errCode))
                {
                    if (errCode != "00")
                    {
                        AnswerCompleate?.Invoke(CheckState, result);

                        return result;
                    }
                    else
                    {
                        byte isOpen = data[2];
                        result.Add(nameof(isOpen), isOpen.ToString());
                        if (isOpen == 1)
                        {
                            int initValue = BitConverter.ToInt32(data, 3);
                            int preValue = BitConverter.ToInt32(data, 7);
                            result.Add(nameof(initValue), initValue.ToString());
                            result.Add(nameof(preValue), preValue.ToString());
                        }

                        AnswerCompleate?.Invoke(CheckState, result);

                        return result;
                    }
                }

            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }

            return result;
        }

        private Dictionary<string, string> CloseCheckAnsw(byte[] data)
        {
            var result = BaseAnsw(data, false);

            if (result.TryGetValue("err", out string? value))
            {
                if (value == "cmdId")
                {
                    return result;
                }
            }

            try
            {
                if (result.TryGetValue("errCode", out string? errCode))
                {
                    if (errCode != "00")
                    {
                        AnswerCompleate?.Invoke(CheckClose, result);

                        return result;
                    }
                    else
                    {
                        int surrender = BitConverter.ToInt32(data, 2);

                        result.Add(nameof(surrender), surrender.ToString());
                        result.Add("Сдача: ", surrender.ToString());

                        AnswerCompleate?.Invoke(CheckClose, result);

                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                return new Dictionary<string, string> { { "Ошибка: ", e.Message } };
            }

            return result;
        }

        private Dictionary<string, string> BaseAnsw(byte[] data, bool invoke = true)
        {
            try
            {
                if (_lastCmdId != _currentCmdId)
                {
                    RetryCMD();
                    return new Dictionary<string, string>{ {"err" , "cmdId"} };
                }
                else
                {
                    byte cmd = data[0];
                    byte errCode = data[1];
                    if (errCode != ENET_OK)
                    {
                        var result = new Dictionary<string, string>
                        {
                            { nameof(errCode), errCode.ToString("x2") },
                            { "Ошибка: ", ErrCodes[errCode] },
                            { "cmd", cmd.ToString("x2")}
                        };

                        if (errCode == ENET_SYS_ERROR)
                        {
                            ushort wideErrCode = BitConverter.ToUInt16(data, 2);
                            byte msgFlag = data[4];
                            result.Add(nameof(wideErrCode), wideErrCode.ToString("x2"));
                            if(msgFlag == 1)
                            {
                                string answ = Encoding.GetEncoding(1251).GetString(data, 5, 65);
                                result.Add(nameof(answ), answ);
                            }

                        }


                        if (invoke) AnswerCompleate?.Invoke(cmd, result);

                        return result;
                    }
                    else
                    {
                        var result = new Dictionary<string, string>
                        {
                            { nameof(errCode), errCode.ToString("x2") },
                            { "Ответ: ", ErrCodes[errCode] },
                            { "cmd", cmd.ToString("x2")}
                        };
                        if (invoke) AnswerCompleate?.Invoke(cmd, result);

                        return result;
                    }
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
