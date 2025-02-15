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
            Items.Clear();
            InitializeComponent();

            var item = Items.FirstOrDefault(x => x.Title == _homeMenuItem.Text);
            if (item != null)
            {
                Items.Remove(item);
                Items.Insert(0, item);
            }
        }

        private void OnLoaded(object? sender, EventArgs e)
        {
            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;

            InitAppData();
        }
        private async void InitAppData()
        {
            await Task.Delay(500);
            await AppData.CheckBLEPermission();
            await AppData.CheckLockationPermission();

            await AppData.CheckBLE();
            await AppData.CheckLockation();

            await AppData.Initialize();
        }

        private void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            Initialize();
        }

        private async void HomeMIClicked(object sender, EventArgs e)
        {
            await GoToAsync("//Remote", true);
            FlyoutIsPresented = false;
        }

        public void Quit()
        {
            OnBackButtonPressed();
        }

        public async Task CreateDevicePrefabMenu(IDevice device)
        {
            var page = new CreateDeviceBanner(new Models.Banners.CreateDeviceViewModel(device));
            page.Disappearing += (async (sender, e) =>
            {
                if (!page.Result) await AppData.SpecialDisconnect();
            });
            await Navigation.PushModalAsync(page);
        }
    }
}
