using Plugin.BluetoothClassic.Abstractions;

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
            FillDevices();
        }

        private void FillDevices()
        {
            var adapter = DependencyService.Resolve<IBluetoothAdapter>();
            ListOfDevices.ItemsSource = adapter.BondedDevices;
        }

        private async void ListOfDevices_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var device = (BluetoothDeviceModel)e.SelectedItem;
            if (device != null)
            {
                await Navigation.PushAsync(new DeviceViewPage(device));
            }

            ListOfDevices.SelectedItem = null;
        }
    }
}