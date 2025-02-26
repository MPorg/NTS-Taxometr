using Android.Hardware.Camera2;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Data.DataBase.Objects;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Services;

namespace TaxometrMauiMvvm.Models.Pages;

public partial class PrintViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _blockBannerIsVisible;
    [ObservableProperty]
    private string _devicesBtnText;
    [ObservableProperty]
    private bool _isLoaded;

    private bool _isAppearing = false;
    private bool _isWaitApearing = false;

    public event Action<TabBarViewModel> TabBarInjection;
    public PrintViewModel()
    {
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
    private async void PrintReceiptOrSwitchMode(string key)
    {
        AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
        IsLoaded = true;
        await AppData.PrintReceiptOrSwitchMode(key);
    }

    private bool _isFirstInit = true;
    public void OnAppearing()
    {
        _isAppearing = true;
        if (_isFirstInit)
        {
            _isFirstInit = false;
            TabBarInjection?.Invoke(AppData.TabBarViewModel);
            //AppData.TabBarViewModel.Transit(from: TabBarViewModel.Transition.Remote | TabBarViewModel.Transition.Drive);
            //AppData.TabBarViewModel.Transit(to: TabBarViewModel.Transition.Print);
        }
        AppData.TabBarViewModel.Transit(to: TabBarViewModel.Transition.Print);
        
        if (BlockBannerIsVisible)
        {
            _isWaitApearing = true;
            return;
        }

        _isWaitApearing = false;
        AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
        IsLoaded = true;
        AppData.Provider.SentTaxState(true);
    }

    public void OnDisappearing()
    {
        _isAppearing = false;
        AppData.TabBarViewModel.Transit(from: TabBarViewModel.Transition.Print);
    }

    private async void OnProvider_AnswerCompleate(byte cmd, Dictionary<string, string> answer)
    {
        switch (cmd)
        {
            case ProviderExtentions.TaxState: await ReadTaxState(answer); break;
            case ProviderExtentions.SwitchMode: await ReadSwitchMode(answer); break;
        }
    }

    private async Task ReadSwitchMode(Dictionary<string, string> answer)
    {
        IsLoaded = false;
    }

    private async Task ReadTaxState(Dictionary<string, string> answer)
    {
        if (answer.TryGetValue("menuState", out string? menuState))
        {
            if (!string.IsNullOrEmpty(menuState))
            {
                AppData.Provider.AnswerCompleate -= OnProvider_AnswerCompleate;
                if (menuState == "0")
                {
                    Debug.WriteLine("_________________________Main menu mode is active_________________________");
                    IsLoaded = false;
                    return;
                }
                else
                {
                    AppData.Provider.AnswerCompleate += OnProvider_AnswerCompleate;
                    IsLoaded = true;
                    await AppData.PrintReceiptOrSwitchMode("M");
                }
            }
        }
    }
}
