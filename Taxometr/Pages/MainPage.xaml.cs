using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using Taxometr.DataBase;
using Taxometr.DataBase.Objects;
using Taxometr.Pages;

namespace Taxometr
{


    public partial class MainPage : ContentPage
    {
        double popUpOffset = 1000;
        private readonly TaxometerLocalDB taxometerLocalDB;

        public MainPage(TaxometerLocalDB taxometerLocalDB)
        {
            InitializeComponent();
            this.taxometerLocalDB = taxometerLocalDB;
            Start();
        }
        private void Start()
        {
            PopUpMenu.TranslationX = -popUpOffset;
            LoadAppTheme();
        }

        private async void Update()
        {
            await LoadAppTheme();

            Thread.Sleep(100);
        }

        public async Task LoadAppTheme()
        {
            Property property = await taxometerLocalDB.GetPropertyByNameAsync("App theme"); 
            int.TryParse(property.PropertyValue, out int value);
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
#pragma warning disable CA1416 // Проверка совместимости платформы
            switch (value)
            {
                case 0:
                    Application.Current.UserAppTheme = AppTheme.Unspecified;
                    LoadLight();
                    break;
                case 1:
                    Application.Current.UserAppTheme = AppTheme.Light;
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.LightContent);
                    LoadLight();
                    break;
                case 2:
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(StatusBarStyle.DarkContent);
                    LoadDark();
                    break;
            }
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
#pragma warning restore CA1416 // Проверка совместимости платформы
        }

        private void LoadLight()
        {
            MenuBtn.Source = ImageSource.FromFile("menu_l.svg");
        }
        
        private void LoadDark()
        {
            MenuBtn.Source = ImageSource.FromFile("menu_d.svg");
        }

        private void StartStopBtn_Clicked(object sender, EventArgs e)
        {
            if (StartStopBtn.Text == "Начать поездку")
            {
                DebugLabel.Text = "Поездка начата";
                StartStopBtn.Text = "Завершить поездку";
            }
            else if (StartStopBtn.Text == "Завершить поездку")
            {
                DebugLabel.Text = "Поездка завершена";
                StartStopBtn.Text = "Начать поездку";
            }
        }

        private async Task PopUp()
        {
            if (PopUpMenu.TranslationX >= 0)
            {
                await PopUpMenu.TranslateTo(-popUpOffset, 0, 250, Easing.CubicIn);
                CancelPopUpBtn.IsEnabled = false;
            }
            else
            {
                await PopUpMenu.TranslateTo(0, 0, 250, Easing.CubicOut);
                CancelPopUpBtn.IsEnabled = true;
            }
        }

        private async void MenuBtn_Clicked(object sender, EventArgs e)
        {
            await PopUp();
        }

        private async void DevicesBtn_Clicked(object sender, EventArgs e)
        {
            await PopUp();
            //await Navigation.PushAsync(new DevicesPageBLE());
        }

        private async void SettingsBtn_Clicked(object sender, EventArgs e)
        {
            await PopUp();
            SettingsPage settingsPage = new SettingsPage(taxometerLocalDB);
            settingsPage.Unloaded += async (_, _) =>
            {
                await LoadAppTheme();
            };
            await Navigation.PushModalAsync(settingsPage);
        }

        private async void AboutBtn_Clicked(object sender, EventArgs e)
        {
            await PopUp();
            await DisplayAlert(
                "                           О программе",
                "\t" +
                "                           ОДО \"НТС\"\r\n\r\n" +
                "      Программа \"Таксометр\" предназначена для\r\n" +
                "        подключения и управления устройством\r\n" +
                "                \"Таксометр Геомер-122\"",
                "ОК");
        }

        private async void CancelPopUpBtn_Clicked(object sender, EventArgs e)
        {
            if (PopUpMenu.TranslationX >= 0)
            {
                await PopUpMenu.TranslateTo(-popUpOffset, 0, 250, Easing.CubicIn);
                CancelPopUpBtn.IsEnabled = false;
            }

        }
    }

}
