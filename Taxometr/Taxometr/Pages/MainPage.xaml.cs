using System;
using System.Diagnostics;
using Taxometr.Data;
using Taxometr.Services;
using Xamarin.Essentials;
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
            AppData.ConnectionLost += OnConnectionLost;
		}

        protected override async void OnAppearing()
        {
            if (AppData.BLEAdapter.ConnectedDevices.Count > 0)
            {
                SwitchBan();
                try
                {
                    AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Main, await AppData.Properties.GetAdminPassword());
                }
                catch { }
            }
            else SwitchBan(true);
        }

        private void OnAutoconnectionCompleated()
        {
            SwitchBan();
        }

        private void OnConnectionLost()
        {
            SwitchBan(true);
        }

        private void SwitchBan(bool enable = false)
        {
            BanLayout.IsVisible = enable;
            InfoBtn.IsEnabled = !enable;
            if (!enable)
            {
                //DeviceName.Text = AppData.AutoConnectDevice.Name;
            }
        }

        private void OnDevicesBtnClicked(object sender, EventArgs e)
        {
			AppData.MainMenu.OpenDevicesPage();
        }

        private void OnStatusSCNOBtnClicked(object sender, EventArgs e)
        {
            //_provider.SentFlc(ProviderBLE.FlcType.RDY);
            AppData.Provider.SentScnoState(true);
        }

        private void OnInfoBtnClicked(object sender, EventArgs e)
        {
            AppData.Provider.SentTaxInfo(true);
        }

        private void OnEmitBtnClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button != null)
            {
                switch (button.CommandParameter)
                {
                    case "C":
                        AppData.Provider.EmmitButton(ProviderBLE.ButtonKey.C);
                        break;
                    case "OK":
                        AppData.Provider.EmmitButton(ProviderBLE.ButtonKey.OK);
                        break;
                    case "Up":
                        AppData.Provider.EmmitButton(ProviderBLE.ButtonKey.Up);
                        break;
                    case "Down":
                        AppData.Provider.EmmitButton(ProviderBLE.ButtonKey.Down);
                        break;
                    case "Num_1":
                        AppData.Provider.EmmitButton(ProviderBLE.ButtonKey.Num_1);
                        break;
                    case "Num_2":
                        AppData.Provider.EmmitButton(ProviderBLE.ButtonKey.Num_2);
                        break;
                }
            }
        }
    }
}