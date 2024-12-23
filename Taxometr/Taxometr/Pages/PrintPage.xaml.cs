using System;
using Taxometr.Data;
using Taxometr.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintPage : ContentPage
    {
        public PrintPage()
        {
            InitializeComponent();
            ZReceipt.Text = "Сменный \"Z\" отчёт";
            XReceipt.Text = "\"X\" отчёт";
            AppData.AutoconnectionCompleated += OnAutoconnectionCompleated;
            AppData.ConnectionLost += OnConnectionLost;
        }
        protected override async void OnAppearing()
        {
            if (AppData.BLEAdapter.ConnectedDevices.Count > 0) SwitchBan();
            else SwitchBan(true);
            try
            {
                AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Main, await AppData.Properties.GetAdminPassword());
            }
            catch { }
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
        }
        private void OnDevicesBtnClicked(object sender, EventArgs e)
        {
            AppData.MainMenu.OpenDevicesPage();
        }

        private async void OnZReceiptClicked(object sender, EventArgs e)
        {
            try
            {
                AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Z, await AppData.Properties.GetAdminPassword());
            }
            catch { }
        }
        private async void OnXReceiptClicked(object sender, EventArgs e)
        {
            try
            {
                AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.X, await AppData.Properties.GetAdminPassword());
            }
            catch { }
        }
    }
}