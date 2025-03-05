using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using TaxometrMauiMvvm.Models.Cells;

namespace TaxometrMauiMvvm.Models.Pages;

public partial class SearchViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<SearchedDeviceViewModel> _devices;

    [RelayCommand]
    private void Tapped(SearchedDeviceViewModel device)
    {
        device.TappedCommand.Execute(this);
    }

    [RelayCommand]
    private void Add(IDevice device)
    {
        if (device == null || device.Id == Guid.Empty || string.IsNullOrEmpty(device.Name)) return;

        foreach (var deviceViewModel in _devices)
        {
            if (deviceViewModel.Device.Id == device.Id) return;
        }

        var sdm = new SearchedDeviceViewModel(device);
        Devices.Add(sdm);
    }

    public SearchViewModel()
    {
        Devices = new ObservableCollection<SearchedDeviceViewModel>();
    }
}