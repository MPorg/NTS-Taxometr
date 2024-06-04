using System;
using System.Threading.Tasks;
using Taxometr.DataBase.Objects;
using Taxometr.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void SettingsMI_Clicked(object sender, System.EventArgs e)
        {
            SettingsPage settingsPage = new SettingsPage();
            settingsPage.Unfocused += (_, _1) =>
            {
                InitializeComponent();
            };
            await Navigation.PushModalAsync(settingsPage);
        }
    }
}