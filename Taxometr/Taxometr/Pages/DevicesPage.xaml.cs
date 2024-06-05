using Plugin.BluetoothClassic.Abstractions;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Taxometr.DataBase.Tmp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicesPage : ContentPage
    {
        public DevicesPage()
        {
            InitializeComponent();
            Start();
        }

        private async void Start()
        {
            if (await EnableBluetooth())
                FillDevices();
        }

        private async Task<bool> EnableBluetooth()
        {
            if (AppData.Adapter.Enabled) return true;
            if (await DisplayAlert("Разрешение", "Приложение запрашивает разрешение на включение BLUETOOTH", "ОК", "Отмена")) AppData.Adapter.Enable();
            if (AppData.Adapter.Enabled) return true;
            return false;
        }

        private void FillDevices()
        {
            ListOfDevices.ItemsSource = AppData.Adapter.BondedDevices;
        }

        private async void ListOfDevices_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var device = (BluetoothDeviceModel)e.SelectedItem;
            if (device != null)
            {
                await Navigation.PushAsync(new DeviceView(device));
            }
            ListOfDevices.SelectedItem = null;
        }

        private async void RefreshPage(object sender, System.EventArgs e)
        {
            FillDevices();
            await Task.Delay(1000);
            Refresh.IsRefreshing = false;
        }
    }
}