using System.Diagnostics;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Models.Pages;
using TaxometrMauiMvvm.Services;

namespace TaxometrMauiMvvm.Views.Pages;

public partial class RemotePage : ContentPage, IQueryAttributable
{
    RemoteViewModel _viewModel;
    public RemotePage(RemoteViewModel viewModel, IToastMaker toastMaker, ISettingsManager settingsManager, IKeyboard keyboard)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        AppData.SetDependencyServices(toastMaker, settingsManager, keyboard);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!AppData.InitializationCompleate) return;
        Debug.WriteLine("_____________________________Remote page onAppearing_____________________________");
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
            bool cleare = false;
            string message = "";
            Dictionary<ProviderBLE.ButtonKey, Action> onKeysPressed = new Dictionary<ProviderBLE.ButtonKey, Action>();
            ProviderBLE.ButtonKey enableButtons = ProviderBLE.ButtonKey.None;

            if (query.TryGetValue(nameof(cleare), out var clr))
            {
                if (cleare is bool c)
                {
                    cleare = c;
                    if (cleare = true)
                    {
                        _viewModel.Clear();
                        return;
                    }
                }
            }


            if (query.TryGetValue(nameof(message), out var msg) && query.TryGetValue(nameof(onKeysPressed), out var onKey) && query.TryGetValue(nameof(enableButtons), out var enable))
            {
                if (msg is string m && onKey is Dictionary<ProviderBLE.ButtonKey, Action> okp && enable is ProviderBLE.ButtonKey enbl)
                {
                    message = m;
                    onKeysPressed = okp;
                    enableButtons = enbl;
                }
            }

            SetMessage(message, onKeysPressed, enableButtons);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
