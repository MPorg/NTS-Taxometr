using Android.Content;
using Android.Content.PM;
using Java.Lang;
using Plugin.BLE.Abstractions.Contracts;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Views.Banners;
using Exception = System.Exception;

namespace TaxometrMauiMvvm
{
    public partial class MainMenu : Shell
    {
        public MainMenu()
        {
            Loaded += OnLoaded;

            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();

            var item = Items.FirstOrDefault(x => x.Title == _homeMenuItem.Text);
            if (item != null)
            {
                Items.Remove(item);
                Items.Insert(0, item);
            }
        }

        private async void OnLoaded(object? sender, EventArgs e)
        {
            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }

        private async void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            await DisplayAlert("Смена темы приложения", "Необходимо перезапустить приложение", "ОК");
            Restart();
        }

        private async void HomeMIClicked(object sender, EventArgs e)
        {
            await GoToAsync("//Remote", true);
            FlyoutIsPresented = false;
        }

        public async void GoTo(string rote)
        {
            await GoToAsync(rote, true);
        }

        public void Quit()
        {
            Application.Current?.Quit();
        }

        public void Restart()
        {
#if ANDROID
            var context = Platform.AppContext;
            PackageManager packageManager = context.PackageManager;
            Intent intent = packageManager.GetLaunchIntentForPackage(context.PackageName);
            ComponentName componentName = intent.Component;
            Intent mainIntent = Intent.MakeRestartActivityTask(componentName);
            mainIntent.SetPackage(context.PackageName);
            context.StartActivity(mainIntent);
            Runtime.GetRuntime().Exit(0);
#endif
        }

        public async Task<bool> CreateDevicePrefabMenu(IDevice device)
        {
            try
            {
                bool result = false;
                bool isCompleate = false;
                var page = new CreateDeviceBanner(new Models.Banners.CreateDeviceViewModel(device));
                page.Disappearing += (async (sender, e) =>
                {
                    result = page.Result;
                    if (!result) await AppData.SpecialDisconnect();

                    isCompleate = true;
                });
                await Navigation.PushModalAsync(page);
                while (!isCompleate)
                {
                    await Task.Delay(100);
                }
                return result;
            }
            catch (Exception ex)
            {
                AppData.ShowToast(ex.Message);
            }
            return false;
        }

        private async void Link_Clicked(object sender, EventArgs e)
        {
            try
            {
                Uri uri = new Uri("https://nts-shop.by/contacts/kontakty-gomel/");
                await Browser.Default.OpenAsync(uri, new BrowserLaunchOptions { Flags = BrowserLaunchFlags.None, LaunchMode = BrowserLaunchMode.SystemPreferred, TitleMode = BrowserTitleMode.Default});
            }
            catch (Exception ex)
            {
                // An unexpected error occurred. No browser may be installed on the device.
            }
        }
    }
}
