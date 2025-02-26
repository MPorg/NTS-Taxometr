using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Models.Pages;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class PrintPage : ContentPage
{
    PrintViewModel _viewModel;
    public PrintPage(PrintViewModel printViewModel)
    {
        InitializeComponent();
        BindingContext = printViewModel;
        _viewModel = printViewModel;
        _viewModel.TabBarInjection += OnTabBarInjection;
    }

    private void OnTabBarInjection(TabBarViewModel tabBar)
    {
        TabBar.Inject(tabBar);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (AppData.InitializationCompleate)
        {
            Debug.WriteLine("_____________________________Print page onAppearing_____________________________");
            _viewModel.OnAppearing();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (AppData.InitializationCompleate)
        {
            Debug.WriteLine("_____________________________Print page onDisappearing_____________________________");
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
}