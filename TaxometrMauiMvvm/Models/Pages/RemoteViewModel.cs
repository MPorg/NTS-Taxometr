using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Data.DataBase.Objects;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Services;

namespace TaxometrMauiMvvm.Models.Pages;

public partial class RemoteViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _blockBannerIsVisible;
    [ObservableProperty]
    private string _devicesBtnText;
    [ObservableProperty]
    private string _message;
    [ObservableProperty]
    private bool _isLoaded;

    public RemoteViewModel()
    {
        //Clear();
        BtnCIsEnabled = true;
        BtnOkIsEnabled = true;
        BtnUpIsEnabled = true;
        BtnDownIsEnabled = true;
        BtnNum_1IsEnabled = true;
        BtnNum_2IsEnabled = true;

        GetBtnText();
        SwitchBan();
        AppData.ConnectionLost += SwitchBan;
        AppData.BLEAdapter.DeviceConnected += ((_, __) => { SwitchBan(); });
    }
    private async void GetBtnText()
    {
        List<DevicePrefab> devices = await (await AppData.TaxometrDB()).Device.GetPrefabsAsync();
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

    private Dictionary<ProviderBLE.ButtonKey, Action> _lastKeysActions = new Dictionary<ProviderBLE.ButtonKey, Action>();
    [RelayCommand]
    private async Task EmmitBtn(string key)
    {
        Action action = null;
        switch (key)
        {
            case "C":
                (await AppData.Provider()).EmitButton(ProviderBLE.ButtonKey.C);
                _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.C, out action);
                break;
            case "OK":
                (await AppData.Provider()).EmitButton(ProviderBLE.ButtonKey.OK);
                _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.OK, out action);
                break;
            case "Up":
                (await AppData.Provider()).EmitButton(ProviderBLE.ButtonKey.Up);
                _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Up, out action);
                break;
            case "Down":
                (await AppData.Provider()).EmitButton(ProviderBLE.ButtonKey.Down);
                _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Down, out action);
                break;
            case "Num_1":
                (await AppData.Provider()).EmitButton(ProviderBLE.ButtonKey.Num_1);
                _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Num_1, out action);
                break;
            case "Num_2":
                (await AppData.Provider()).EmitButton(ProviderBLE.ButtonKey.Num_2);
                _lastKeysActions.TryGetValue(ProviderBLE.ButtonKey.Num_2, out action);
                break;
            default:
                throw new NotImplementedException();
        }
        action?.Invoke();
    }
    public void SetMessage(string message, Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed, ProviderBLE.ButtonKey enableButtons)
    {
        EnableButtons(enableButtons);
        Message = message;
        _lastKeysActions = onKeysPressed;
    }
    public void Clear()
    {
        EnableButtons(ProviderBLE.ButtonKey.Any);
        Message = "";
        _lastKeysActions = new Dictionary<ProviderBLE.ButtonKey, Action>();
    }

    [ObservableProperty]
    private bool _btnCIsEnabled;
    [ObservableProperty]
    private bool _btnOkIsEnabled;
    [ObservableProperty]
    private bool _btnUpIsEnabled;
    [ObservableProperty]
    private bool _btnDownIsEnabled;
    [ObservableProperty]
    private bool _btnNum_1IsEnabled;
    [ObservableProperty]
    private bool _btnNum_2IsEnabled;

    private void EnableButtons(ProviderBLE.ButtonKey enableButtons)
    {
        BtnCIsEnabled = enableButtons.HasFlag(ProviderBLE.ButtonKey.C);
        BtnUpIsEnabled = enableButtons.HasFlag(ProviderBLE.ButtonKey.Up);
        BtnDownIsEnabled = enableButtons.HasFlag(ProviderBLE.ButtonKey.Down);
        BtnOkIsEnabled = enableButtons.HasFlag(ProviderBLE.ButtonKey.OK);
        BtnNum_1IsEnabled = enableButtons.HasFlag(ProviderBLE.ButtonKey.Num_1);
        BtnNum_2IsEnabled = enableButtons.HasFlag(ProviderBLE.ButtonKey.Num_2);
    }

    [RelayCommand]
    public void OnApearing()
    {
        AppData.TabBarViewModel.Transit(to: TabBarViewModel.Transition.Remote);
    }

    [RelayCommand]
    public void OnDisapearing()
    {
        AppData.TabBarViewModel.Transit(from: TabBarViewModel.Transition.Remote);
    }
}
