using System;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Interfaces;
using Taxometr.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrintPage : ContentPage, ICheckedTransition
    {
        public PrintPage()
        {
            InitializeComponent();
            ZReceipt.Text = "Сменный \"Z\" отчёт";
            XReceipt.Text = "\"X\" отчёт";
            AppData.AutoconnectionCompleated += OnAutoconnectionCompleated;
            AppData.ConnectionLost += OnConnectionLost;

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Run(async () =>
                {
                    while (AppData.MainMenu == null)
                    {
                        await Task.Delay(1);
                    }

                    TryTransit -= AppData.MainMenu.OnCheck_TryTransit;

                    TryTransit += AppData.MainMenu.OnCheck_TryTransit;
                });
            });
        }

        public event Action<Type> TryTransit;

        protected override async void OnAppearing()
        {
            LoadingLayout.IsVisible = false;

            if (AppData.BLEAdapter.ConnectedDevices.Count > 0) SwitchBan();
            else SwitchBan(true);
            try
            {
                if (AppData.MainMenu.SwitchingIsBusy)
                {
                    TryTransit?.Invoke(typeof(PrintPage));
                    return;
                }

                AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Main, await AppData.Properties.GetAdminPassword(), true, 2);

                if (AppData.MainMenu != null)
                    AppData.MainMenu.Mode = MainMenu.MenuMode.Print;
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
                AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.Z, await AppData.Properties.GetAdminPassword(), true, 10);

                LoadingLayout.IsVisible = true;
                AppData.MainMenu.SetBusy(true, typeof(PrintPage));
                await Task.Delay(10000);
                AppData.MainMenu.SetBusy(false, typeof(PrintPage));
                LoadingLayout.IsVisible = false;
            }
            catch { }
        }
        private async void OnXReceiptClicked(object sender, EventArgs e)
        {
            try
            {
                AppData.Provider.OpenMenuOrPrintReceipt(ProviderBLE.MenuMode.X, await AppData.Properties.GetAdminPassword(), true, 5);
                LoadingLayout.IsVisible = true;
                AppData.MainMenu.SetBusy(true, typeof(PrintPage));
                await Task.Delay(5000);
                AppData.MainMenu.SetBusy(false, typeof(PrintPage));
                LoadingLayout.IsVisible = false;
            }
            catch { }
        }
    }
}