using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Models.Pages;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class DrivePage : ContentPage
{
    DriveViewModel _viewModel;

    public DrivePage(DriveViewModel driveViewModel)
    {
        InitializeComponent();
        BindingContext = driveViewModel;
        _viewModel = driveViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!AppData.InitializationCompleate) return;
        //Debug.WriteLine("_____________________________Drive page onAppearing_____________________________");

        _viewModel.OnAppearing();
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
}