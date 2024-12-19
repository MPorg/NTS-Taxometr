using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Data.DataBase;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage : ContentPage, IDisposable
    {
        IAdapter _adapter;
        ObservableCollection<DeviceViewCell.DeviceViewCellBinding> _bindings = new ObservableCollection<DeviceViewCell.DeviceViewCellBinding>();
        ObservableCollection<IDevice> _devices = new ObservableCollection<IDevice>();
        IDevice _connectedDevice = null;


        public DevicesPage()
        {
            InitializeComponent();
            _adapter = AppData.BLEAdapter;
            _adapter.DeviceDiscovered += OnAdapterDeviceDiscovered;
            _adapter.DeviceDisconnected += OnAdapterDeviceDisconnected;
            _adapter.DeviceConnected += OnAdapterDeviceConnected;
            _adapter.ScanTimeoutElapsed += OnScanTimeoutElapsed;
            ListOfDevices.ItemsSource = _bindings;
            Start();

        }

        private async void OnAdapterDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            _connectedDevice = e.Device;
            if (!await CheckContainsInSaves())
            {
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    AppData.MainMenu.CreateDevicePrefabMenuAsync(_connectedDevice, (res, serNum, blePass, adminPass, customName, autoconnect) => 
                    {
                        if (res)
                        {
                            SaveDevice(serNum, blePass, adminPass, customName, autoconnect);
                            AppData.Initialize();
                            AppData.ShowToast($"Устройство {_connectedDevice.Name} сохранено");
                        }
                        else
                        {
                            _adapter.DisconnectDeviceAsync(_connectedDevice);
                        }
                    });
                });
            }
            AppData.Debug.WriteLine($"{_connectedDevice.Name} Connected");
            FillDevicesList();
        }

        private async Task<bool> CheckContainsInSaves()
        {
            var prefabs = await AppData.TaxometrDB.DevicePrefabs.GetPrefabsAsync();
            if (prefabs == null) return false;
            foreach ( var prefab in prefabs )
            {
                if (prefab.DeviceId == _connectedDevice.Id) return true;
            }
            return false;
        }

        private async void SaveDevice(string serNum, string blePass, string adminPass, string customName, bool autoConnect)
        {
            if (!await CheckContainsInSaves())
            {
                await AppData.TaxometrDB.DevicePrefabs.CreateAsync(new Data.DataBase.Objects.DevicePrefab(_connectedDevice.Id, serNum, blePass, adminPass, customName, autoConnect));
            }
        }

        private void OnAdapterDeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (e.Device == _connectedDevice)
            {
                AppData.Debug.WriteLine($"{_connectedDevice.Name} Disconnected");
                _connectedDevice = null;
                FillDevicesList();
            }
        }

        private async void Start()
        {
            await AppData.CheckLocation();
            await AppData.CheckBLE();
            Refresh.IsRefreshing = true;
        }

        private void OnAdapterDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (e.Device != null && !String.IsNullOrEmpty(e.Device.Name))
            {
                if (!_devices.Contains(e.Device)) _devices.Add(e.Device);
                FillDevicesList();
            }
        }

        private async Task StartScaning()
        {
            _devices.Clear();

            foreach (var device in _adapter.ConnectedDevices)
            {
                _devices.Add(device);
                _connectedDevice = device;
                FillDevicesList();
            }

            if (!_adapter.IsScanning)
            {
                await _adapter.StartScanningForDevicesAsync();
            }
        }

        private async void FillDevicesList()
        {
            _bindings.Clear();
            if (_connectedDevice != null)
            {
                var b = new DeviceViewCell.DeviceViewCellBinding(_connectedDevice);
                _bindings.Add(b);
                if (await CheckContainsInSaves())
                {
                    b.ConnectionCompleate = true;
                }
            }
            foreach (var d in _devices)
            {
                if (d == _connectedDevice) continue;
                var bind = new DeviceViewCell.DeviceViewCellBinding(d);
                _bindings.Add(bind);
            }
        }

        private void OnListOfDevicesItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is DeviceViewCell.DeviceViewCellBinding selection)
            {
                if (selection.Device.State == DeviceState.Connected)
                {
                    _adapter.DisconnectDeviceAsync(_connectedDevice);
                }
                else if (selection.Device.State == DeviceState.Disconnected)
                {
                    try
                    {
                        var connectParameters = new ConnectParameters(false, true);
                        _adapter.ConnectToDeviceAsync(selection.Device, connectParameters);
                    }
                    catch (Exception ex)
                    {
                        AppData.ShowToast
                            (
                                $"Не удалось подключиться к устройству: {selection.Name ?? "N/A"}"
                            );
                    }
                }
            }
        }

        public async void RefreshPage(object sender, System.EventArgs e)
        {
            await StartScaning();
        }

        private void OnRefreshBtnClicked(object sender, EventArgs e)
        {
            Refresh.IsRefreshing = true;
        }

        private void OnScanTimeoutElapsed(object sender, System.EventArgs e)
        {
            Refresh.IsRefreshing = false;
            if (_bindings.Count == 0)
            {
                Refresh.IsRefreshing = true;
            }
        }

        public void Dispose()
        {
            _adapter.DeviceDiscovered -= OnAdapterDeviceDiscovered;
            _adapter.DeviceDisconnected -= OnAdapterDeviceDisconnected;
            _adapter.DeviceConnected -= OnAdapterDeviceConnected;
            _adapter.ScanTimeoutElapsed -= OnScanTimeoutElapsed;
        }
    }
}