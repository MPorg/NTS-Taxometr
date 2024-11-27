using System;
using System.Collections.Generic;

using Plugin.BluetoothLE;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPageBluetoothLE : ContentPage
    {
        private readonly IAdapter _adapter;
        private readonly List<IDevice> _devices = new List<IDevice>();
        private List<IDevice> _lastDevices = new List<IDevice>();
        private List<DeviceViewCellBluetoothLE.DeviceViewCellBindingBluetoothLE> _bindings = new List<DeviceViewCellBluetoothLE.DeviceViewCellBindingBluetoothLE>();

        public DevicesPageBluetoothLE()
        {
            InitializeComponent();
        }

        public async void RefreshPage(object sender, System.EventArgs e)
        {
            //await StartScaning();
        }

        private void OnRefreshBtnClicked(object sender, EventArgs e)
        {
            Refresh.IsRefreshing = true;
        }

        private async void OnListOfDevicesItemTapped(object sender, ItemTappedEventArgs e)
        {
        }
    }
}