using TaxometrMauiMvvm.Data.DataBase;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Platforms.Android.Services;
using CommunityToolkit.Mvvm.Messaging;
using TaxometrMauiMvvm.Services;
using System.Diagnostics;
using System.Reflection;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class SplashScreen : ContentPage
{
    App _app;

    private ImageSource _image;
    private INotificationService _notificationService;
    private IBackgroundConnectionController _backgroundConnectionController;

    public ImageSource Image
    {
        get => _image;
        set
        {
            _image = value;
            OnPropertyChanged(nameof(Image));
        }
    }

	public SplashScreen(App currentApp, INotificationService notificationService, IBackgroundConnectionController backgroundConnectionController)
	{
        _notificationService = notificationService;
        _backgroundConnectionController = backgroundConnectionController;
        _app = currentApp;
        BindingContext = this;
        GetRandomImage();
		InitializeComponent();
    }

    private void GetRandomImage()
    {
        int rnd = new Random().Next(1, 5);


        Image = ImageSource.FromFile($"Resources/Images/Taxis/taxi_{rnd}.jpeg");
    }

    public List<string> GetImages()
    {
        // Получаем путь к папке Pngs
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = AppDomain.CurrentDomain.BaseDirectory; // Замените YourNamespace на ваш фактический пространство имен

        // Получаем все ресурсы в папке
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(resourcePath) && (name.EndsWith(".jpg") || name.EndsWith(".jpeg")));

        // Преобразуем имена ресурсов в список
        var imageList = resourceNames.Select(name => name.Substring(resourcePath.Length + 1)).ToList();

        return imageList;
    }

    public async Task GetDB(IToastMaker toastMaker)
    {
        TaxometrDB dB = await AppData.TaxometrDB();
        if (dB == null)
        {
            toastMaker.Show("Разрешение не выдано");
            return;
        }
        else
        {
            if (AppData.MainMenu != null)
            {
                _notificationService.CloseNotifications();
                switch (AppData.State)
                {
                    case AppData.AppState.BackgroundConnected:
                        AppData.State = AppData.AppState.NormalConnected;
                        _backgroundConnectionController.Stop();
                        break;
                    case AppData.AppState.BackgroundDisconnected:
                        AppData.State = AppData.AppState.NormalDisconnected;
                        break;
                }
                
            }

            MainMenu menu = new MainMenu();
            AppData.MainMenu = menu;
            await Task.Delay(3000);
            await InitAppData();
            _app.Windows[0].Page = menu;
        }
    }
    private async Task InitAppData()
    {
        await AppData.CheckBLEPermission();
        await AppData.CheckLockationPermission();
        await AppData.CheckNotificationPermission();

        await AppData.CheckBLE(this);
        await AppData.CheckLockation(this);

        await AppData.Initialize();
    }
}