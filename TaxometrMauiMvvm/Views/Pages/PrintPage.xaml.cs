using System.Diagnostics;
using TaxometrMauiMvvm.Data;
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
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!AppData.InitializationCompleate) return;
        Debug.WriteLine("_____________________________Print page onAppearing_____________________________");
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