using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BluetoothClassic.Abstractions;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Data.DataBase.Objects;
using Taxometr.Interfaces;
using Taxometr.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage : ContentPage
    {
        private readonly IAdapter _adapter;
        private readonly List<IDevice> _devices = new List<IDevice>();
        private List<IDevice> _lastDevices = new List<IDevice>();
        private List<DeviceViewCell.DeviceViewCellBinding> _bindings = new List<DeviceViewCell.DeviceViewCellBinding>();

        public DevicesPage()
        {
            InitializeComponent();
            _adapter = AppData.BLEAdapter;
            Start();
        }

        private async void Start()
        {
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
            _adapter.DeviceConnected += OnAdapterDeviceConnected;
            _adapter.ScanTimeoutElapsed += OnScanTimeoutElapsed;

            await AppData.CheckLocation();
            await AppData.CheckBLE();

            foreach (var device in _adapter.ConnectedDevices)
            {
                _devices.Add(device);
            }
            FillDevicesList();
            await StartScaning();
        }

        private void OnAdapterDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            var device = e.Device;
            foreach (var b in _bindings)
            {
                if (b.Device.Id == device.Id)
                {
                    b.Connecting = false;
                    break;
                }
            }

            //добавить всплывающее условие
            SaveDevice(device);
        }

        private async void SaveDevice(IDevice device)
        {
            List<IService> services = new List<IService>(await device.GetServicesAsync());
            await AppData.TaxometrDB.Devices.UpdateAsync(new DeviceModel(device.Name, device.Id, services[3].Id));
            AppData.AutoConnectDevice = device;
        }

        private void OnScanTimeoutElapsed(object sender, System.EventArgs e)
        {
            Refresh.IsRefreshing = false;
        }

        private void FillDevicesList()
        {
            List<DeviceViewCell.DeviceViewCellBinding> bindings = new List<DeviceViewCell.DeviceViewCellBinding>();
            foreach (var d in _devices)
            {
                bindings.Add(new DeviceViewCell.DeviceViewCellBinding(d));
            }
            _bindings = bindings;
            ListOfDevices.ItemsSource = bindings.ToArray();
        }

        private void OnDeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            if (e.Device != null && !string.IsNullOrEmpty(e.Device.Name))
            {
                _devices.Add(e.Device);
                FillDevicesList();
            }
        }

        private async Task StartScaning()
        {
            ListOfDevices.ItemsSource = _bindings;

            _devices.Clear();

            if (!_adapter.IsScanning)
            {
                await _adapter.StartScanningForDevicesAsync();
            }

            foreach (var device in _adapter.ConnectedDevices)
            {
                _devices.Add(device);
            }

            FillDevicesList();
        }

        private async void OnListOfDevicesItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (ListOfDevices.ItemsSource != null)
                {
                    foreach (var i in ListOfDevices.ItemsSource)
                    {
                        var s = i as DeviceViewCell.DeviceViewCellBinding;
                        s.HideButtons.Execute(s);
                    }
                }

                if (e.Item is DeviceViewCell.DeviceViewCellBinding selection)
                {
                    if (selection.Device.State == DeviceState.Connected)
                    {
                        await _adapter.DisconnectDeviceAsync(selection.Device);
                        await StartScaning();
                    }
                    else
                    {
                        try
                        {
                            var connectParameters = new ConnectParameters(false, true);
                            await _adapter.ConnectToDeviceAsync(selection.Device, connectParameters);
                            await StartScaning();
                        }
                        catch
                        {
                            selection.Connecting = false;
                            AppData.ShowToast($"Не удалось подключиться к устройству: {selection.Name ?? "N/A"}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
    }
}