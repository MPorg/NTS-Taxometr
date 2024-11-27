using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxometr.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadSettings();
        }

        private async void LoadSettings()
        {
            AutoconnectSwitch.IsToggled = await AppData.Properties.GetAutoconnect();

            if (AppData.AutoConnectDevice == null)
            {
                SwitchAutoConnectParameters(false);
            }
            else
            {
                SwitchAutoConnectParameters(true);
                AutoConnectDeviceName.Text = AppData.AutoConnectDevice.Name;
                SerialNumberEntry.Text = await AppData.Properties.GetSerialNumber();
            }
        }

        private void SwitchAutoConnectParameters(bool enable = true)
        {
            switch (enable)
            {
                case false:
                    AutoConnectParameter.IsEnabled = false;
                    DisableAutoConnectPanel.IsVisible = true;
                    SerialNumberParameter.IsEnabled = false;
                    DisableSerialPanel.IsVisible = true;
                    break;
                case true:
                    AutoConnectParameter.IsEnabled = true;
                    DisableAutoConnectPanel.IsVisible = false;
                    SerialNumberParameter.IsEnabled = true;
                    DisableSerialPanel.IsVisible = false;
                    break;
            }
        }

        private async void OnAutoconnectSwitchToggled(object sender, ToggledEventArgs e)
        {
            bool toggled = e.Value;

            await AppData.Properties.SaveAutoconnect(toggled);
        }

        private async void OnSerialNumberEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            string value = SerialNumberEntry.Text;

            await AppData.Properties.SaveSerialNumber(value);
        }
    }
}