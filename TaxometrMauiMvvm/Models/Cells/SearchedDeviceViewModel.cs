using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using static TaxometrMauiMvvm.Data.DataBase.TaxometrDB;

namespace TaxometrMauiMvvm.Models.Cells;

public partial class SearchedDeviceViewModel : ObservableObject
{
    [ObservableProperty]
    private string _customName;

    [ObservableProperty]
    private string _connectionStateRU;

    private bool _isConnection = false;

    [RelayCommand]
    private async Task Tapped()
    {
        try
        {
            if (AppData.ConnectedDP != null)
            {
                bool isThis = AppData.ConnectedDP.DeviceId == _device.Id;
                await AppData.SpecialDisconnect();
                if (isThis) return;
            }

            if (await AppData.ConnectToDevice(_device))
            {
                _isConnection = true;
                if (!await CheckContainsInSaves())
                {
                    CreateNew();
                }
            }
        }
        catch (Exception ex)
        {
            _isConnection = false;
            AppData.ShowToast(ex.Message);
        }
    }

    private async void CreateNew()
    {
        bool result = await AppData.CreateNewPrefab(_device);
        _isConnection = false;

        if (result)
        {
            AppData.MainMenu.GoTo("//Remote");
        }
    }

    private IDevice _device;
    public IDevice Device => _device;

    public SearchedDeviceViewModel(IDevice device)
    {
        _device = device;
        CustomName = _device.Name;
        StartUpdateState();
    }

    private void StartUpdateState()
    {
        CheckConnectionState();
        Application.Current?.Dispatcher.StartTimer(TimeSpan.FromSeconds(0.5f), new Func<bool>(() =>
        {
            CheckConnectionState();
            return true;
        }));
    }



    private void CheckConnectionState()
    {
        if (_device != null)
        {
            if (_isConnection) ConnectionStateRU = "Подключение" + AppData.Dots;
            else ConnectionStateRU = DeviceViewModelExtentions.ConnectionStateRU[_device.State.ToString()];
        }
    }

    private async Task<bool> CheckContainsInSaves()
    {
        var prefabs = await (await AppData.TaxometrDB()).Device.GetPrefabsAsync();
        if (prefabs == null || prefabs.Count == 0) return false;
        foreach (var prefab in prefabs)
        {
            if (prefab.DeviceId == _device?.Id) return true;
        }
        return false;
    }
}
