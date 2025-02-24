using TaxometrMauiMvvm.Data.DataBase;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Platforms.Android.Services;
using CommunityToolkit.Mvvm.Messaging;
using TaxometrMauiMvvm.Services;
using System.Diagnostics;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class SplashScreen : ContentPage
{
    App _app;
	public SplashScreen(App currentApp)
	{
        _app = currentApp;
		InitializeComponent();
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

        await AppData.CheckBLE(this);
        await AppData.CheckLockation(this);

        await AppData.Initialize();
    }
}