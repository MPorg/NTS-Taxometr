using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Pages;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : Shell
    {
        private DevicesTabbedPage _devicesPage;
        private SettingsPage _settingsPage;
        Action<bool, string, string, string, string, bool> _result;

        public MainMenu()
        {
            InitializeComponent();
        }

        internal async void Start()
        {
            await AppData.CheckLocation();
            await AppData.CheckBLE();
        }

        public void CreateDevicePrefabMenuAsync(IDevice device, Action<bool, string, string, string, string, bool> result)
        {
            _result = result;
            var page = new CreateDevicePrefab(device, OnCreationPageCompleated);
            _creationPage = page;
            page.Disappearing += OnCreatingPageDisappearing;
            Navigation.PushModalAsync(page);
        }

        Page _creationPage;
        private void OnCreationPageCompleated(bool res, string serNum, string blePass, string adminPass, string customName, bool autoConnect)
        {
            _creationPage.Disappearing -= OnCreatingPageDisappearing;
            _creationPage.Navigation.PopModalAsync();
            _result(res, serNum, blePass, adminPass, customName, autoConnect);   
        }

        private void OnCreatingPageDisappearing(object sender, EventArgs e)
        {
            _result(false, "", "", "", "", false);
        }

        public async void OpenDevicesPage()
        {
            if (_devicesPage == null)
            {
                _devicesPage = new DevicesTabbedPage();
            }
            await Navigation.PushAsync(_devicesPage);
        }
        public async void OpenSettingsPage()
        {
            if (_settingsPage == null)
            {
                _settingsPage = new SettingsPage();
            }
            await Navigation.PushAsync(_settingsPage);
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