using Android.Health.Connect.DataTypes.Units;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Data.DataBase.Objects;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Services;

namespace TaxometrMauiMvvm.Models.Pages;

public partial class DriveViewModel : ObservableObject
{
    [ObservableProperty]
    string _title;
    [ObservableProperty]
    private bool _blockBannerIsVisible;
    [ObservableProperty]
    private string _devicesBtnText;
    [ObservableProperty]
    private bool _checkIsOpened;
    [ObservableProperty]
    private bool _shiftIsOpened;
    [ObservableProperty]
    private bool _isLoaded;

    private bool _isAppearing = false;
    private bool _isWaitApearing = false;

    private int _openedCheckStartSum = 0;
    private int _openedCheckPreviousSum = 0;

    public event Action<TabBarViewModel> TabBarInjection;

    public DriveViewModel()
    {
        _title = "Смена";
        IsLoaded = false;
        GetBtnText();
        SwitchBan();
        AppData.ConnectionLost += SwitchBan;
        AppData.BLEAdapter.DeviceConnected += ((_, __) => { SwitchBan(); });
    }
    private async void GetBtnText()
    {
        List<DevicePrefab> devices = await (await AppData.TaxometrDB()).DevicePrefabs.GetPrefabsAsync();
        if (devices.Count > 0) DevicesBtnText = "Устройства";
        else DevicesBtnText = "Поиск";
    }

    [RelayCommand]
    private void DevicesBtn()
    {
        if (DevicesBtnText == "Устройства") Shell.Current.GoToAsync("//Favorites", true);
        else if (DevicesBtnText == "Поиск") Shell.Current.GoToAsync("//Search", true);
    }

    [RelayCommand]
    private void SwitchBan()
    {
        if (AppData.BLEAdapter != null && AppData.BLEAdapter.ConnectedDevices.Count > 0)
        {
            BlockBannerIsVisible = false;
            if (_isWaitApearing && _isAppearing)
            {
                OnAppearing();
            }
        }
        else
        {
            BlockBannerIsVisible = true;
        }
    }

    [RelayCommand]
    private async void DeposWithdrawCash(string key)
    {
        var cashMethod = key switch
        {
            "D" => ProviderBLE.CashMethod.Deposit,
            "W" => ProviderBLE.CashMethod.Withdrawal,
            _ => throw new NotImplementedException(),
        };

        (await AppData.Provider()).AnswerCompleate += OnProvider_AnswerCompleate;
        await AppData.GetDeposWithdrawBanner(cashMethod);
    }

    [RelayCommand]
    private async Task OpenShift()
    {
        (await AppData.Provider()).AnswerCompleate += OnProvider_AnswerCompleate;
        (await AppData.Provider()).OpenShift();
    }

    [RelayCommand]
    private async Task CopyCheck()
    {
        (await AppData.Provider()).CopyCheck();
    }

    [RelayCommand]
    private async Task OpenCheck()
    {
        await AppData.GetOpenCheckBanner();
    }

    bool _waitCloseCheck = false;
    [RelayCommand]
    private async Task CloseCheck()
    {
        AppData.CloseCheckBannerAnswer += AppData_CloseCheckBannerAnswer;
        await AppData.GetCloseCheckBanner(_openedCheckStartSum.ToString(), _openedCheckPreviousSum.ToString());
    }

    private void AppData_CloseCheckBannerAnswer(bool result)
    {
        AppData.CloseCheckBannerAnswer -= AppData_CloseCheckBannerAnswer;
        _waitCloseCheck = result;
    }

    [RelayCommand]
    private async Task CancelCheck()
    {
        (await AppData.Provider()).BreakCheck();
        await Task.Delay(500);
        await OnAppearing();
    }

    private bool _isFirstInit = true;
    public async Task OnAppearing()
    {
        _isAppearing = true;
        if (_isFirstInit)
        {
            _isFirstInit = false;
            TabBarInjection?.Invoke(AppData.TabBarViewModel);
            //AppData.TabBarViewModel.Transit(from: TabBarViewModel.Transition.Remote | TabBarViewModel.Transition.Print);
            //AppData.TabBarViewModel.Transit(to: TabBarViewModel.Transition.Drive);
        }
        AppData.TabBarViewModel.Transit(to: TabBarViewModel.Transition.Drive);

        if (BlockBannerIsVisible)
        {
            _isWaitApearing = true;
            return;
        }
        _isWaitApearing = false;
        (await AppData.Provider()).AnswerCompleate += OnProvider_AnswerCompleate;
        IsLoaded = true;
        if (_waitCloseCheck)
        {
            _waitCloseCheck = false;
        }
        else
        {
            (await AppData.Provider()).SentTaxState(true);
        }
    }

    public void OnDisappearing()
    {
        _isAppearing = false;
        AppData.TabBarViewModel.Transit(from: TabBarViewModel.Transition.Drive);
        IsLoaded = false;
    }

    private async void OnProvider_AnswerCompleate(byte cmd, Dictionary<string, string> answer)
    {
        switch (cmd)
        {
            case ProviderExtentions.TaxState: await ReadTaxState(answer); break;
            case ProviderExtentions.ShiftState: await ReadShiftInfo(answer); break;
            case ProviderExtentions.CheckState: await ReadCheckState(answer); break;
            case ProviderExtentions.CheckClose: await ReadCheckClose(answer); break;
            case ProviderExtentions.ShiftOpen: await ReadOpenShift(answer); break;
        }
    }

    private async Task ReadOpenShift(Dictionary<string, string> answer)
    {
        Debug.WriteLine("READ OPEN SHIFT");
        await CheckDataMessage();
        /*if (answer.TryGetValue("surrender", out string? surrender))
        {
            (await AppData.Provider()).AnswerCompleate -= OnProvider_AnswerCompleate;
            if (!string.IsNullOrEmpty(surrender))
            {
                await SurrenderMessage(surrender);
            }
        }*/
    }

    private async Task ReadCheckClose(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("surrender", out string? surrender))
        {
            (await AppData.Provider()).AnswerCompleate -= OnProvider_AnswerCompleate;
            if (!string.IsNullOrEmpty(surrender))
            {
                await SurrenderMessage(surrender);
            }
        }
    }

    private bool _shiftOpenMessage = true;
    private async Task ReadTaxState(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("menuState", out string? menuState))
        {
            if (!string.IsNullOrEmpty(menuState))
            {
                (await AppData.Provider()).AnswerCompleate -= OnProvider_AnswerCompleate;
                if (menuState == "1")
                {
                    Debug.WriteLine("_________________________Drive mode is active_________________________");

                    (await AppData.Provider()).AnswerCompleate += OnProvider_AnswerCompleate;
                    _shiftOpenMessage = false;
                    (await AppData.Provider()).SentShiftInfo(true);
                    //return;
                }
                else
                {
                    await AppData.PrintReceiptOrSwitchMode("D");

                    (await AppData.Provider()).AnswerCompleate += OnProvider_AnswerCompleate;
                    (await AppData.Provider()).SentShiftInfo(true);
                }
            }
        }
    }

    private async Task ReadCheckState(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("isOpen", out string? isOpen))
        {
            if (!string.IsNullOrEmpty(isOpen))
            {
                Debug.WriteLine(isOpen);
                IsLoaded = false;
                (await AppData.Provider()).AnswerCompleate -= OnProvider_AnswerCompleate;
                if (isOpen == "1")
                {
                    CheckIsOpened = true;
                }
                else if (isOpen == "0")
                {
                    CheckIsOpened = false;
                    _openedCheckStartSum = 0;
                    _openedCheckPreviousSum = 0;
                }
            }
        }
        if (answer.TryGetValue("initValue", out string? initValueStr))
        {
            if (!string.IsNullOrEmpty(initValueStr))
            {
                Debug.WriteLine(initValueStr);
                (await AppData.Provider()).AnswerCompleate -= OnProvider_AnswerCompleate;
                _openedCheckStartSum = int.Parse(initValueStr);
            }
        }
        if (answer.TryGetValue("preValue", out string? preValueStr))
        {
            if (!string.IsNullOrEmpty(preValueStr))
            {
                Debug.WriteLine(preValueStr);
                (await AppData.Provider()).AnswerCompleate -= OnProvider_AnswerCompleate;
                _openedCheckPreviousSum = int.Parse(preValueStr);
            }
        }
    }

    private async Task ReadShiftInfo(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("shiftState", out string? shiftState))
        {
            if (!string.IsNullOrEmpty(shiftState))
            {
                (await AppData.Provider()).AnswerCompleate -= OnProvider_AnswerCompleate;
                if (shiftState == "1")
                {
                    ShiftIsOpened = true;
                    (await AppData.Provider()).AnswerCompleate += OnProvider_AnswerCompleate;
                    (await AppData.Provider()).SentCheckState(true);
                }
                else if (shiftState == "0")
                {
                    ShiftIsOpened = false;
                    if (!_shiftOpenMessage)
                    {
                        _shiftOpenMessage = true;
                        IsLoaded = false;
                        return;
                    }
                    await OpenShiftMessage();
                }
            }
        }
    }

    private async Task OpenShiftMessage()
    {
        string message = "Смена не открыта.\r\nОткрыть смену?\r\n[OK] - ДА [C] - НЕТ";
        Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed = new Dictionary<ProviderBLE.ButtonKey, Action>
        {
            {ProviderBLE.ButtonKey.OK, new Action(async ()=>
            {
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{"IsLoaded", true }});
                await Task.Delay(2500);
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{"IsLoaded", false}});
                await CheckDataMessage();
                //AppData.MainMenu.GoToAsync("//Drive", true);
            }) },
            {ProviderBLE.ButtonKey.C, new Action(async ()=>
            {
                Debug.WriteLine("C");
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{ "clear", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true, new ShellNavigationQueryParameters{{"IsLoaded", false}});
            }) }
        };
        ProviderBLE.ButtonKey enableButtons = ProviderBLE.ButtonKey.OK | ProviderBLE.ButtonKey.C;
        ShellNavigationQueryParameters parameters = new ShellNavigationQueryParameters
        {
            {nameof(message), message},
            {nameof(onKeysPressed), onKeysPressed},
            {nameof(enableButtons), enableButtons}
        };

        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            Debug.WriteLine($"\r\nMessage: {message}");
            await AppData.MainMenu.GoToAsync("//Remote", true, parameters);
        });
    }

    private async Task CheckDataMessage()
    {
        string message = "Подтвердите дату и время\r\n[OK] - Подтвердить [C] - Не открывать смену\r\n[2] - Синхронизировать из СКНО";
        Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed = new Dictionary<ProviderBLE.ButtonKey, Action>
        {
            {ProviderBLE.ButtonKey.OK, new Action(async ()=>
            {
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{ "clear", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true, new ShellNavigationQueryParameters{{"IsLoaded", false}});
                (await AppData.Provider()).SentCheckState(true);
            }) },
            {ProviderBLE.ButtonKey.C, new Action(async ()=>
            {
                Debug.WriteLine("C");
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{ "clear", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true, new ShellNavigationQueryParameters{{"IsLoaded", false}});
            }) },
            {ProviderBLE.ButtonKey.Num_2, new Action(() =>
            {

            }) }
        };
        ProviderBLE.ButtonKey enableButtons = ProviderBLE.ButtonKey.OK | ProviderBLE.ButtonKey.C | ProviderBLE.ButtonKey.Num_2;
        ShellNavigationQueryParameters parameters = new ShellNavigationQueryParameters
        {
            {nameof(message), message},
            {nameof(onKeysPressed), onKeysPressed},
            {nameof(enableButtons), enableButtons}
        };
        Debug.WriteLine($"\r\nMessage: {message}");
        await AppData.MainMenu.GoToAsync("//Remote", true, parameters);
    }

    private async Task SurrenderMessage(string surrender)
    {
        if (surrender == "0")
        {
            surrender = "0,00";
        }
        else if (surrender.Length == 1)
        {
            surrender = "0,0" + surrender;
        }
        else if (surrender.Length == 2)
        {
            surrender = "0," + surrender;
        }
        else
        {
            List<char> chars = new List<char>();
            for (int i = 0; i < surrender.Length; i++)
            {
                chars.Add(surrender[i]);
                if (i == surrender.Length - 3) chars.Add(',');
            }
            surrender = new string(chars.ToArray());
        }

        string message = $"Сдача: {surrender}";
        Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed = new Dictionary<ProviderBLE.ButtonKey, Action>
        {
            {ProviderBLE.ButtonKey.OK, new Action(async ()=>
            {
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{ "clear", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true);
            }) }
        };
        ProviderBLE.ButtonKey enableButtons = ProviderBLE.ButtonKey.OK;
        ShellNavigationQueryParameters parameters = new ShellNavigationQueryParameters
        {
            {nameof(message), message},
            {nameof(onKeysPressed), onKeysPressed},
            {nameof(enableButtons), enableButtons}
        };
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            Debug.WriteLine($"\r\nMessage: {message}");
            await AppData.MainMenu.GoToAsync("//Remote", true, parameters);
        });
    }
}
