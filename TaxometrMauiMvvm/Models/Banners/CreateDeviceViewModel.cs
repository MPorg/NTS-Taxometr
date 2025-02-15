using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Fonts.IconFont;

namespace TaxometrMauiMvvm.Models.Banners;

public partial class CreateDeviceViewModel : ObservableObject
{
    private IDevice _device;

    public event Action<bool> Result;

    [ObservableProperty]
    private string _deviceName = "";

    [ObservableProperty]
    private string _serialNumber = "";

    [ObservableProperty]
    private string _blePassword = "";

    [ObservableProperty]
    private string _adminPassword = "";

    [ObservableProperty]
    private bool _autoconnect = false;

    [ObservableProperty]
    private bool _isPassword = true;

    [ObservableProperty]
    private string _showHideGlyph;

    [ObservableProperty]
    private bool _okBtnIsActive;
    public void CheckOkBtn()
    {
        OkBtnIsActive = SerialNumber.Length == 8 && BlePassword.Length == 6 && AdminPassword.Length == 6 && !string.IsNullOrEmpty(DeviceName);
    }

    [RelayCommand]
    private void ShowHidePassword()
    {
        IsPassword = !IsPassword;
        ShowHideGlyph = IsPassword ? Icons.IconEyeHide : Icons.IconEyeShow;
    }

    [RelayCommand]
    private async Task Create()
    {
        Debug.WriteLine($"Save {_device.Id},{SerialNumber}, {BlePassword}, {AdminPassword}, {DeviceName}, {Autoconnect}");
        await AppData.TaxometrDB.DevicePrefabs.CreateAsync(new Data.DataBase.Objects.DevicePrefab(_device.Id, SerialNumber, BlePassword, AdminPassword, DeviceName, Autoconnect));
        Result?.Invoke(true);
    }

    [RelayCommand]
    private void Cancel()
    {
        Result?.Invoke(false);
    }

    private CreateDeviceViewModel()
    {
        IsPassword = true;
        ShowHideGlyph = IsPassword ? Icons.IconEyeHide : Icons.IconEyeShow;
        CheckOkBtn();
        PropertyChanged += CreateDeviceViewModel_PropertyChanged;
    }

    private void CreateDeviceViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(OkBtnIsActive)) return;
        CheckOkBtn();
    }

    public CreateDeviceViewModel(IDevice device) : this()
    {
        _device = device;
        DeviceName = device.Name;
    }
}
