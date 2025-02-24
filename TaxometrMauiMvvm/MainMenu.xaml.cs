using Plugin.BLE.Abstractions.Contracts;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Views.Banners;

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

        private void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            Items.Clear();
            Initialize();
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
            OnBackButtonPressed();
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
                    isCompleate = true;
                    if (!page.Result) await AppData.SpecialDisconnect();
                    result = false;
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

    }
}
