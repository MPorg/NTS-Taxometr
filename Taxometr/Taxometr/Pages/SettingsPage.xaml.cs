using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxometr.DataBase;
using Taxometr.DataBase.Objects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Taxometr.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {

        TaxometerLocalDB taxometerLocalDB;
        public SettingsPage()
        {
            InitializeComponent();
            LoadChangesAsync();
        }

        private async Task LoadChangesAsync()
        {
            SetDefaultValues();
            try
            {
                taxometerLocalDB = App.TaxometerLocalDB;
                List<Property> properties = await taxometerLocalDB.GetPropertiesAsync();
                if (properties == null || properties.Count == 0)
                {
                    await taxometerLocalDB.CreatePropertyAsync(new Property("App theme", ThemePicker.SelectedIndex.ToString()));
                }
                Property appTheme = await taxometerLocalDB.GetPropertyByNameAsync("App theme");
                int.TryParse(appTheme.PropertyValue, out int intValue);
                ThemePicker.SelectedIndex = intValue;
            }
            catch
            {
                taxometerLocalDB = App.TaxometerLocalDB;
                List<Property> properties = await taxometerLocalDB.GetPropertiesAsync();
                if (properties == null || properties.Count == 0)
                {
                    await taxometerLocalDB.CreatePropertyAsync(new Property("App theme", ThemePicker.SelectedIndex.ToString()));
                }
                Property appTheme = await taxometerLocalDB.GetPropertyByNameAsync("App theme");
                int.TryParse(appTheme.PropertyValue, out int intValue);
                ThemePicker.SelectedIndex = intValue;
            }

            Cancel.Text = "Назад";
            Save.IsEnabled = false;
        }

        private void SetDefaultValues()
        {
            ThemePicker.SelectedIndex = 0;
            SetTheme();
        }

        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SwitchButtonsTitle();
            SetTheme();
        }
        private void SetTheme()
        {
            Picker picker = ThemePicker;
            int i = picker.SelectedIndex;
            switch (i)
            {
                case 0: Application.Current.UserAppTheme = OSAppTheme.Unspecified;
                    break;
                case 1:
                    Application.Current.UserAppTheme = OSAppTheme.Light;
                    break;
                case 2:
                    Application.Current.UserAppTheme = OSAppTheme.Dark;
                    break;
            }
        }

        private void SwitchButtonsTitle()
        {
            Cancel.Text = "Отмена";
            Save.IsEnabled = true;
        }

        private async void Clancel_Clicked(object sender, EventArgs e)
        {
            await LoadChangesAsync();
            await Navigation.PopModalAsync();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            await SaveChangesAsync();
            await Navigation.PopModalAsync();
        }

        private async Task SaveChangesAsync()
        {
            Property appTheme = await taxometerLocalDB.GetPropertyByNameAsync("App theme");
            appTheme.PropertyValue = ThemePicker.SelectedIndex.ToString();
            await taxometerLocalDB.UpdatePropertyAsync(appTheme);
        }
    }
}