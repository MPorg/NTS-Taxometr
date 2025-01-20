using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Interfaces;
using Taxometr.Pages;
using Taxometr.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : Shell
    {
        public enum MenuMode
        {
            Remote,
            Drive,
            Print
        }

        private MenuMode _mode = MenuMode.Remote;
        public MenuMode Mode { get => _mode; set { _mode = value; } }
        private bool _switchingIsBusy = false;
        public bool SwitchingIsBusy { get => _switchingIsBusy; private set { _switchingIsBusy = value; } }

        private DevicesTabbedPage _devicesPage;
        //private SettingsPage _settingsPage;
        Action<bool, string, string, string, string, bool> _result;

        public MainMenu()
        {
            InitializeComponent();
        }

        public void OnCheck_TryTransit(Type type)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (!SwitchingIsBusy) return;

                if ((type == typeof(RemotePage) && Mode == MenuMode.Remote) ||
                    (type == typeof(DrivePage) && Mode == MenuMode.Drive) ||
                    (type == typeof(PrintPage) && Mode == MenuMode.Print)) return;

                switch (Mode)
                {
                    case MenuMode.Remote:
                        GoToAsync("///Remote", false);
                        break;
                    case MenuMode.Drive:
                        GoToAsync("///Drive", false);
                        break;
                    case MenuMode.Print:
                        GoToAsync("//Print", false);
                        break;
                }
            });
        }

        public void SetBusy(bool busy, Type type)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (busy == false)
                {
                    DriveTab.IsEnabled = true;
                    PrintTab.IsEnabled = true;
                    return;
                }

                if (type == typeof(DrivePage))
                {
                    PrintTab.IsEnabled = false;
                }
                else if (type == typeof(PrintPage))
                {
                    DriveTab.IsEnabled = false;
                }
            });
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
        /*public async void OpenSettingsPage()
        {
            if (_settingsPage == null)
            {
                _settingsPage = new SettingsPage();
            }
            await Navigation.PushAsync(_settingsPage);
        }*/

        private void OnDevicesMIClicked(object sender, EventArgs e)
        {
            OpenDevicesPage();
        }

        /*private void OnSettingsMIClicked(object sender, EventArgs e)
        {
            OpenSettingsPage();
        }*/
    }
}