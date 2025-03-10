using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Models.Pages;
using TaxometrMauiMvvm.Services;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class DrivePage : ContentPage, IQueryAttributable
{
    DriveViewModel _viewModel;

    public DrivePage(DriveViewModel driveViewModel)
    {
        InitializeComponent();
        BindingContext = driveViewModel;
        _viewModel = driveViewModel;
        _viewModel.TabBarInjection += OnTabBarInjection;
    }
    private void OnTabBarInjection(TabBarViewModel tabBar)
    {
        TabBar.Inject(tabBar);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
        if (AppData.InitializationCompleate)
        {

            Debug.WriteLine("_____________________________Drive page onAppearing_____________________________");
            _viewModel.OnAppearing();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (AppData.InitializationCompleate)
        {

            Debug.WriteLine("_____________________________Drive page onDisappearing_____________________________");
            _viewModel.OnDisappearing();
        }
    }

    private bool _backButtonToast = false;
    protected override bool OnBackButtonPressed()
    {
        if (_backButtonToast)
        {
            return false;
        }
        else
        {
            AppData.ShowToast("Нажмите ещё раз для выхода из приложения");
            _backButtonToast = true;
            Dispatcher.StartTimer(TimeSpan.FromSeconds(3), new Func<bool>(() =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    _backButtonToast = false;
                });
                return false;
            }));
        }
        return true;
    }
    private void FlayoutBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            bool IsLoaded = false;
            if (query.TryGetValue(nameof(IsLoaded), out var loaded))
            {
                if (loaded is bool load)
                {
                    _viewModel.IsLoaded = load;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}