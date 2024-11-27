using System;
using Taxometr.Data;
using Taxometr.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : Shell
    {
        public MainMenu()
        {
            InitializeComponent();
            AppData.MainMenu = this;
        }

        public async void OpenDevicesPage()
        {
            var devicesPage = new DevicesPage();
            devicesPage.Disappearing += (_, _1) => { Navigation.RemovePage(devicesPage); };
            await Navigation.PushAsync(devicesPage);
        }
        public async void OpenSettingsPage()
        {
            var settingsPage = new SettingsPage();
            settingsPage.Disappearing += (_, _1) => { Navigation.RemovePage(settingsPage); };
            await Navigation.PushAsync(settingsPage);
        }

        private void OnDevicesMIClicked(object sender, EventArgs e)
        {
            OpenDevicesPage();
        }

        private void OnSettingsMIClicked(object sender, EventArgs e)
        {
            OpenSettingsPage();
        }
    }
}