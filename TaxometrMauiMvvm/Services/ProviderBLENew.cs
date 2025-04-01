using Plugin.BLE.Abstractions.Contracts;
using static TaxometrMauiMvvm.Services.ProviderBLE;
using System.Collections.Concurrent;
using Plugin.BLE.Abstractions.EventArgs;
using TaxometrMauiMvvm.Data;
using System.Diagnostics;

namespace TaxometrMauiMvvm.Services;

public class ProviderBLENew
{/*
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

    private readonly List<byte> _answerBufer = new();

    private string? _serialNumber;
    private string? _key;

    private int _sentToBleRetry = 0;

    private readonly Timer _statesTimer = new Timer(0);

    #endregion

    #region Init

    public ProviderBLENew()
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
        if (_retryStop)
        {
            if (state == ProviderState.SentFR || state == ProviderState.ReciveFLC_1)
            {
                await ReadFR();
            }
            else
            {
                Debug.WriteLine($"Extreem Clear {_cmdQueue.Count}");
                _cmdQueue.Clear();
                _next = true;
            }
            return;
        }
        if (state == ProviderState.SentFR || state == ProviderState.ReciveFLC_1) await ReadFR();
        else if (state == ProviderState.SentData_0) RetryCMD();
    }

    #endregion

    #region BLE Controll


    private async void OnCharacterValueUpdated(object? sender, CharacteristicUpdatedEventArgs e)
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

    #endregion*/
}
