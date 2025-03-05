using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Cells;

namespace TaxometrMauiMvvm.Models.Pages;

public partial class SavedViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _selectionMode = false;

    [ObservableProperty]
    private ObservableCollection<SavedDeviceViewModel> _devices;

    private ObservableCollection<SavedDeviceViewModel> _selectedDevices = new ObservableCollection<SavedDeviceViewModel>();

    [RelayCommand]
    private void Tapped(SavedDeviceViewModel device)
    {
        if (!SelectionMode) return;
        //device.TappedCommand.Execute(this); 
        if (_selectedDevices.Contains(device))
        {
            _selectedDevices.Remove(device);
        }
        else
        {
            _selectedDevices.Add(device);
        }
    }

    [RelayCommand]
    private void EnterToEditMode(SavedDeviceViewModel? firstSelectedItem)
    {
        SelectionMode = true;
        if (firstSelectedItem != null)
        {
            if (_selectedDevices.Count > 0 && _selectedDevices.Contains(firstSelectedItem)) return;

            firstSelectedItem.LongSelectionIsVisible = false;
            _selectedDevices.Add(firstSelectedItem);
            if (_devices.Count > 0)
            {
                foreach (var device in _devices)
                {
                    device.LongSelectionIsVisible = false;
                    device.SettingsBntIsVisible = false;
                    device.IsSelectionMode = true;
                    device.TryOpenCloseCommand.Execute(true);
                }
            }
        }
    }

    [RelayCommand]
    private void EnterToBaseMode()
    {
        SelectionMode = false;
        _selectedDevices.Clear();

        foreach (var device in _devices)
        {
            device.DeselectCommand.Execute(true);
            device.LongSelectionIsVisible = true;
            device.IsSelectionMode = false;
            device.SettingsBntIsVisible = true;
        }
    }

    public SavedViewModel()
    {
        _devices = new ObservableCollection<SavedDeviceViewModel>();
        GetDeviceList();
        _selectedDevices.CollectionChanged += OnSelectedDevicesChanged;
    }

    private void OnSelectedDevicesChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        try
        {
            int oldCount = e.OldItems == null ? 0 : e.OldItems.Count;
            int newCount = e.NewItems == null ? 0 : e.NewItems.Count;

            if (_selectedDevices.Count == 0)
            {
                //Clear
                foreach (var item in e.OldItems)
                {
                    if (item is SavedDeviceViewModel model)
                    {
                        model.DeselectCommand.Execute(this);
                    }
                }

                foreach (var device in _devices)
                {
                    device.LongSelectionIsVisible = true;
                    device.IsSelectionMode = false;
                    device.SettingsBntIsVisible = true;
                }

                EnterToBaseMode();
            }

            if (oldCount > newCount)
            {
                //Remove
                List<SavedDeviceViewModel> cleareOldItems = new List<SavedDeviceViewModel>();
                foreach (var item in e.OldItems)
                {
                    if (item is SavedDeviceViewModel model)
                    {
                        if (newCount > 0 && e.NewItems.Contains(item)) continue;
                        cleareOldItems.Add(model);
                    }
                }
                foreach (var item in cleareOldItems)
                {
                    item.DeselectCommand.Execute(this);
                }
            }
            else if (oldCount < newCount)
            {

                //Add
                List<SavedDeviceViewModel> cleareNewItems = new List<SavedDeviceViewModel>();
                foreach (var item in e.NewItems)
                {
                    if (item is SavedDeviceViewModel model)
                    {
                        if (oldCount > 0 && e.OldItems.Contains(item)) continue;
                        cleareNewItems.Add(model);
                    }
                }

                foreach (var item in cleareNewItems)
                {
                    item.SelectCommand.Execute(this);
                }
            }
            else
            {
                //Unknown
            }
        }
        catch (Exception ex)
        {
            Debug.Write(ex.Message);
            if (ex.InnerException != null)
            {
                Debug.Write(ex.InnerException.Message);
            }
        }
    }

    public async void GetDeviceList()
    {
        var devices = await (await AppData.TaxometrDB()).DevicePrefabs.GetPrefabsAsync();
        if (devices.Count > 0)
        {
            foreach (var d in devices)
            {
                if (d != null)
                {
                    bool cont = false;
                    foreach (var item in _devices)
                    {
                        if (item.Device.DeviceId == d.DeviceId) cont = true;
                    }
                    if (cont) continue;

                    var dd = new SavedDeviceViewModel(d, this);
                    _devices.Add(dd);
                }
            }
        }


        /*for (int i = 0; i < 2; i++)
        {
            _devices.Add(new SavedDeviceViewModel(this));
        }*/
    }

    [RelayCommand]
    private void SelectAll()
    {
        Debug.WriteLine($"{_selectedDevices.Count} {_devices.Count}");
        if (_selectedDevices.Count == _devices.Count)
        {
            EnterToBaseMode();
            return;
        }

        if (!SelectionMode) 
            EnterToEditMode(null);
        foreach (var d in _devices)
        {
            if (!_selectedDevices.Contains(d))
            {
                _selectedDevices.Add(d);
            }
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        int count = _selectedDevices.Count;
        string s = count switch
        {
            1 => $"{_selectedDevices[0].CustomName}",
            2 => "два устройства",
            3 => "три устройства",
            4 => "четыре устройства",
            _ => $"{count} устройств"
        };

        if (! await AppData.MainMenu.DisplayAlert("Удалить", $"Вы уверены, что хотите удалить {s}", "Да", "Нет")) return;
        if (_selectedDevices.Count > 0)
        {
            foreach (var device in _selectedDevices)
            {
                if (_devices.Contains(device)) _devices.Remove(device);
                if (AppData.ConnectedDP.DeviceId == device.Device.DeviceId) await AppData.SpecialDisconnect();
                await (await AppData.TaxometrDB()).DevicePrefabs.DeleteAsync(device.Device);
            }
        }
        EnterToBaseMode();
    }
}
