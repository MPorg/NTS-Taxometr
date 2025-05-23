﻿using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Data.DataBase.Objects;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage_Trash : ContentPage
    {
        private readonly IAdapter _adapter;
        private readonly List<IDevice> _devices = new List<IDevice>();
        private List<IDevice> _lastDevices = new List<IDevice>();
        private List<DeviceViewCell.DeviceViewCellBinding> _bindings = new List<DeviceViewCell.DeviceViewCellBinding>();

        private TabbedPage _tabbedPage;

        public DevicesPage_Trash(TabbedPage tabbedPage)
        {
            InitializeComponent();
            _tabbedPage = tabbedPage;
            _adapter = AppData.BLEAdapter;
            Start();
        }

        private async void Start()
        {
            _adapter.DeviceDiscovered += OnDeviceDiscovered;
            _adapter.DeviceConnected += OnAdapterDeviceConnected;
            _adapter.DeviceDisconnected += OnAdapterDeviceDisconnected;
            _adapter.ScanTimeoutElapsed += OnScanTimeoutElapsed;

            await AppData.CheckLocation();
            await AppData.CheckBLE();

            foreach (var device in _adapter.ConnectedDevices)
            {
                _devices.Add(device);
            }
            FillDevicesList();
            Refresh.IsRefreshing = true;
            await StartScaning();
        }

        private async void OnAdapterDeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            AppData.Debug.WriteLine("Disconnected");
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await StartScaning();
            });
        }

        private async void OnAdapterDeviceConnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            AppData.Debug.WriteLine("Connected");
            var device = e.Device;
            foreach (var b in _bindings)
            {
                if (b.Device.Id == device.Id)
                {
                    break;
                }
            }

            /*CreateDevicePrefab prefab = new CreateDevicePrefab();
            _tabbedPage.Navigation.PushModalAsync(prefab);*/

            /*Device.InvokeOnMainThreadAsync(() =>
            {
            });*/
            //добавить всплывающее условие
            SaveDevice(device);
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await StartScaning();
            });

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
            ListOfDevices.ItemsSource = _bindings.ToArray();

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
                AppData.Debug.WriteLine("Item tapped");
                /*if (ListOfDevices.ItemsSource != null)
                {
                    foreach (var i in ListOfDevices.ItemsSource)
                    {
                        var s = i as DeviceViewCell.DeviceViewCellBinding;
                        s.HideButtons.Execute(s);
                    }
                }*/

                if (e.Item is DeviceViewCell.DeviceViewCellBinding selection)
                {
                    AppData.Debug.WriteLine($"{selection.Device.State}");
                    if (selection.Device.State == DeviceState.Connected)
                    {
                        try
                        {
                            AppData.Debug.WriteLine("Disconnecting");
                            await _adapter.DisconnectDeviceAsync(selection.Device);
                        }
                        catch (Exception ex) 
                        {
                            AppData.Debug.WriteLine(ex.Message.ToString());
                        }
                    }
                    else if (selection.Device.State == DeviceState.Disconnected)
                    {
                        AppData.Debug.WriteLine("Connecting");
                        try
                        {
                            var connectParameters = new ConnectParameters(true, true);  
                            await _adapter.ConnectToDeviceAsync(selection.Device, connectParameters);
                        }
                        catch
                        {
                            AppData.ShowToast($"Не удалось подключиться к устройству: {selection.Name ?? "N/A"}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppData.Debug.WriteLine(ex.Message.ToString());
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