using System;
using Taxometr.Data;
using Taxometr.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrivePage : ContentPage
    {
        public DrivePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            if (AppData.BLEAdapter.ConnectedDevices.Count > 0) SwitchBan();
            else SwitchBan(true);
            try
            {
                AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Drive, await AppData.Properties.GetAdminPassword());
            }
            catch { }
        }

        private void SwitchBan(bool enable = false)
        {
            BanLayout.IsVisible = enable;
        }
        private void OnDevicesBtnClicked(object sender, EventArgs e)
        {
            AppData.MainMenu.OpenDevicesPage();
        }

        private void OnOpenShiftBtnClicked(object sender, EventArgs e)
        {
            AppData.Provider.OpenShift();
        }

        private async void OnDeposCashClicked(object sender, EventArgs e)
        {
            await AppData.GetDeposWithdrawBanner(ProviderBLE.CashMethod.Deposit);
        }
        private async void OnWithdrawCashClicked(object sender, EventArgs e)
        {
            await AppData.GetDeposWithdrawBanner(ProviderBLE.CashMethod.Withdrawal);
        }
    }
}