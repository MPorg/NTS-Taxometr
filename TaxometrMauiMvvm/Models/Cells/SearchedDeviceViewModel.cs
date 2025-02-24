using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using TaxometrMauiMvvm.Data;
using static TaxometrMauiMvvm.Data.DataBase.TaxometrDB;

namespace TaxometrMauiMvvm.Models.Cells;

public partial class SearchedDeviceViewModel : ObservableObject
{
    [ObservableProperty]
    private string _customName;

    [ObservableProperty]
    private string _connectionStateRU;

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

            if (await AppData.ConnectToDevice(_device.Id))
            {
                if (!await CheckContainsInSaves())
                {
                    CreateNew();
                }
            }
        }
        catch (Exception ex)
        {
            AppData.ShowToast(ex.Message);
        }
    }

    private async void CreateNew()
    {
       await AppData.CreateNewPrefab(_device);
    }

    private IDevice _device;
    public IDevice Device => _device;

    public SearchedDeviceViewModel(IDevice device)
    {
        _device = device;
        CustomName = _device.Name;
        ConnectionStateRU = DeviceViewModelExtentions.ConnectionStateRU[_device.State.ToString()];
    }

    private async Task<bool> CheckContainsInSaves()
    {
        var prefabs = await (await AppData.TaxometrDB()).DevicePrefabs.GetPrefabsAsync();
        if (prefabs == null || prefabs.Count == 0) return false;
        foreach (var prefab in prefabs)
        {
            if (prefab.DeviceId == _device?.Id) return true;
        }
        return false;
    }
}
