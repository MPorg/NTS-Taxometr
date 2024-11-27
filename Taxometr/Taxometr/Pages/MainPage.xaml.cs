using System;
using Taxometr.Data;
using Taxometr.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent();
            AppData.AutoconnectionCompleated += OnAutoconnectionCompleated;
		}

        protected override void OnAppearing()
        {
            if (AppData.BLEAdapter.ConnectedDevices.Count > 0) SwitchBan();
            else SwitchBan(true);
        }

        private void OnAutoconnectionCompleated()
        {
            SwitchBan();
        }

        private void SwitchBan(bool enable = false)
        {
            BanLayout.IsVisible = enable;
        }

        private void OnDevicesBtnClicked(object sender, EventArgs e)
        {
			AppData.MainMenu.OpenDevicesPage();
        }

        private async void OnStatusSCNOBtnClicked(object sender, EventArgs e)
        {
            string num = await AppData.Properties.GetSerialNumber();
            ProviderBLE.QuitanceRDY();
        }
    }
}