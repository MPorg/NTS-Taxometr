using AndroidX.Lifecycle;
using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Models.Cells;
using TaxometrMauiMvvm.Models.Pages;
using TaxometrMauiMvvm.Services;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class RemotePage : ContentPage, IQueryAttributable
{
    RemoteViewModel _viewModel;
    public RemotePage(RemoteViewModel viewModel, TabBarViewModel tabBarViewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        TabBar.Inject(tabBarViewModel);
        AppData.TabBarViewModel.Transit(from: TabBarViewModel.Transition.Print);
        AppData.TabBarViewModel.Transit(from: TabBarViewModel.Transition.Drive);
        AppData.TabBarViewModel.Transit(to: TabBarViewModel.Transition.Remote);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
        if (AppData.InitializationCompleate)
        {
            Debug.WriteLine("_____________________________Remote page onAppearing_____________________________");
            _viewModel.OnApearing();
        }
    }

    protected override void OnDisappearing()
    {
        _viewModel.OnDisapearing();
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

    public void SetMessage(string message, Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed, ProviderBLE.ButtonKey enableButtons)
    {
        _viewModel.SetMessage(message, onKeysPressed, enableButtons);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            bool clear = false;
            string message = "";
            Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed = new Dictionary<ProviderBLE.ButtonKey, Action>();
            ProviderBLE.ButtonKey enableButtons = ProviderBLE.ButtonKey.None;

            if (query.TryGetValue(nameof(clear), out var clr))
            {
                if (clr is bool c)
                {
                    clear = c;
                    if (clear == true)
                    {
                        Debug.WriteLine("Clear");
                        _viewModel.Clear();
                        return;
                    }
                }
            }

            bool IsLoaded = false;
            if (query.TryGetValue(nameof(IsLoaded), out var loaded))
            {
                if (loaded is bool load)
                {
                    _viewModel.IsLoaded = load;
                }
            }


            if (query.TryGetValue(nameof(message), out var msg) && query.TryGetValue(nameof(onKeysPressed), out var onKey) && query.TryGetValue(nameof(enableButtons), out var enable))
            {
                if (msg is string m && onKey is Dictionary<ProviderBLE.ButtonKey, Action> okp && enable is ProviderBLE.ButtonKey enbl)
                {
                    message = m;
                    onKeysPressed = okp;
                    enableButtons = enbl;
                    SetMessage(message, onKeysPressed, enableButtons);
                }
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void FlayoutBtn_Clicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}
