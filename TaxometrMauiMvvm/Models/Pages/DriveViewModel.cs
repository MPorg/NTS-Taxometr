using Android.Health.Connect.DataTypes.Units;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Data.DataBase.Objects;
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

    public DriveViewModel()
    {
        _title = "Смена";
        GetBtnText();
        SwitchBan();
        AppData.ConnectionLost += SwitchBan;
        AppData.BLEAdapter.DeviceConnected += ((_, __) => { SwitchBan(); });
    }
    private async void GetBtnText()
    {
        List<DevicePrefab> devices = await AppData.TaxometrDB.DevicePrefabs.GetPrefabsAsync();
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

        AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
        await AppData.GetDeposWithdrawBanner(cashMethod);
    }

    [RelayCommand]
    private void CopyCheck()
    {
        AppData.Provider.CopyCheck();
    }

    [RelayCommand]
    private async void OpenCheck()
    {
        AppData.Provider.OpenCheck(200, 0);
        await Task.Delay(5000);
        OnAppearing();
    }

    public void OnAppearing()
    {
        AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
        AppData.Provider.SentTaxState(true);
    }

    private void OnProvider_AnswerCompleate(byte cmd, Dictionary<string, string> answer)
    {
        switch (cmd)
        {
            case ProviderExtentions.TaxState: ReadTaxState(answer); break;
            case ProviderExtentions.ShiftState: ReadShiftInfo(answer); break;
            case ProviderExtentions.CheckState: ReadCheckState(answer); break;
        }
    }

    private async void ReadTaxState(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("menuState", out string? menuState))
        {
            if (!string.IsNullOrEmpty(menuState))
            {
                AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                if (menuState == "1")
                {
                    Debug.WriteLine("_________________________Drive mode is active_________________________");
                    //return;
                }
                else
                {
                    await AppData.PrintReceiptOrSwitchMode("D");

                    AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                    AppData.Provider.SentTaxState(true);

                }
            }
        }
        if (answer.TryGetValue("checkState", out string? checkState))
        {
            if (!string.IsNullOrEmpty(checkState))
            {
                if (checkState == "1")
                {
                    AppData.Provider.SentCheckState(true);
                }
                else
                {
                    CheckIsOpened = false;
                    AppData.Provider.SentShiftInfo(true);
                }
            }
        }
    }

    private void ReadCheckState(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("isOpen", out string? isOpen))
        {
            if (!string.IsNullOrEmpty(isOpen))
            {
                AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                if (isOpen == "1")
                {
                    CheckIsOpened = true;
                }
                else
                {
                    CheckIsOpened = false;
                }
            }
        }
    }

    private void ReadShiftInfo(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("shiftState", out string? shiftState))
        {
            if (!string.IsNullOrEmpty(shiftState))
            {
                AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                if (shiftState == "1")
                {
                    return;
                }
                else
                {
                    OpenShiftMessage();
                }
            }
        }
    }

    private async void OpenShiftMessage()
    {
        string message = "Смена не открыта.\r\nОткрыть смену?\r\n[OK] - ДА [C] - НЕТ";
        Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed = new Dictionary<ProviderBLE.ButtonKey, Action>
        {
            {ProviderBLE.ButtonKey.OK, new Action(()=>
            {
                CheckDataMessage();
                //AppData.MainMenu.GoToAsync("//Drive", true);
            }) },
            {ProviderBLE.ButtonKey.C, new Action(async ()=>
            {
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{"cleare", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true);
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
            await AppData.MainMenu.GoToAsync("//Remote", true, parameters);
        });
    }

    private async void CheckDataMessage()
    {
        string message = "Подтвердите дату и время\r\n[OK] - Подтвердить [C] - Не открывать смену\r\n[2] - Синхронизировать из СКНО";
        Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed = new Dictionary<ProviderBLE.ButtonKey, Action>
        {
            {ProviderBLE.ButtonKey.OK, new Action(async ()=>
            {
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{"cleare", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true);
            }) },
            {ProviderBLE.ButtonKey.C, new Action(async ()=>
            {
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{"cleare", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true);
            }) },
            {ProviderBLE.ButtonKey.Num_2, new Action(async ()=>
            {
                await AppData.MainMenu.GoToAsync("//Remote", true, new ShellNavigationQueryParameters{{"cleare", true }});
                await AppData.MainMenu.GoToAsync("//Drive", true);
            }) }
        };
        ProviderBLE.ButtonKey enableButtons = ProviderBLE.ButtonKey.OK | ProviderBLE.ButtonKey.C | ProviderBLE.ButtonKey.Num_2;
        ShellNavigationQueryParameters parameters = new ShellNavigationQueryParameters
        {
            {nameof(message), message},
            {nameof(onKeysPressed), onKeysPressed},
            {nameof(enableButtons), enableButtons}
        };
        await AppData.MainMenu.GoToAsync("//Remote", true, parameters);
    }
}
