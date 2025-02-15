using Android.Hardware.Camera2;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Data.DataBase.Objects;
using TaxometrMauiMvvm.Services;

namespace TaxometrMauiMvvm.Models.Pages;

public partial class PrintViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _blockBannerIsVisible;
    [ObservableProperty]
    private string _devicesBtnText;

    public PrintViewModel()
    {
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
    private async void PrintReceiptOrSwitchMode(string key)
    {
        await AppData.PrintReceiptOrSwitchMode(key);
    }

    public void OnAppearing()
    {
        AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
        AppData.Provider.SentTaxState(true);
    }

    private async void OnProvider_AnswerCompleate(byte cmd, Dictionary<string, string> answer)
    {
        if (cmd == ProviderExtentions.TaxState)
        {
            if (answer.TryGetValue("menuState", out string? menuState))
            {
                if (!string.IsNullOrEmpty(menuState))
                {
                    AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                    if (menuState == "0")
                    {
                        Debug.WriteLine("_________________________Main menu mode is active_________________________");
                        return;
                    }
                    else
                    {
                        await AppData.PrintReceiptOrSwitchMode("M");
                    }
                }
            }
        }
    }
}
