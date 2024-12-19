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
        }

        private void SwitchAutoConnectParameters(bool enable = true)
        {
            AutoConnectParameter.IsEnabled = enable;
            DisableAutoConnectPanel.IsVisible = !enable;
        }

        private async void OnAutoconnectSwitchToggled(object sender, ToggledEventArgs e)
        {
            bool toggled = e.Value;

            await AppData.Properties.SaveAutoconnect(toggled);
        }
    }
}