using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceView : ContentPage
    {
        IDevice _device;
        private readonly List<IService> _servicesList = new List<IService>();

        public class DeviceViewBinding : BindableObject
        {
            public DeviceViewBinding(string text)
            {
                PageTitle = text;
            }

            public string PageTitle { get; set; } = "Неизвестное устройство";
        }

        public DeviceView(IDevice device)
        {
            InitializeComponent();
            _device = device;
            bleDevice.Text = $"Selected BLE device: {_device.Name}";
            deviceId.Text = $"ID: {_device.Id}";
            deviceState.Text = $"State: {_device.State}";
            BindingContext = new DeviceViewBinding(_device.Name);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var servicesList = await _device.GetServicesAsync();

                _servicesList.Clear();
                var servicesStr = new List<string>();
                for (int i = 0; i < servicesList.Count; i++)
                {
                    _servicesList.Add(servicesList[i]);
                    servicesStr.Add(servicesList[i].Name + ", UUID: " + servicesList[i].Id.ToString());
                }
                //FoundBleServs.ItemsSource = servicesStr;
            }
            catch
            {
                await DisplayAlert("Error initializing", "Error initializing UART GATT service.", "OK");
            }
        }

        private void ConnectBtn_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}