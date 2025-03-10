using SQLitePCL;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Models.Pages.SignIn;
using TaxometrMauiMvvm.Platforms.Android.Services;
using TaxometrMauiMvvm.Views.Pages;

namespace TaxometrMauiMvvm
{
    public partial class App : Application
    {
        private IToastMaker _tMaker; 
        INotificationService _notificationService;
        IBackgroundConnectionController _backgroundConnectionController;
        SplashScreen _splashScreen;
        SignInViewModel _signInViewModel;

        public App(SignInViewModel signIn, IToastMaker toastMaker, ISettingsManager settingsManager, IKeyboard keyboard, IBackgroundConnectionController backgroundConnectionController, INotificationService notificationService)
        {
            InitializeComponent();
            _tMaker = toastMaker;
            _notificationService = notificationService;
            _backgroundConnectionController = backgroundConnectionController;
            _signInViewModel = signIn;
            AppData.SetDependencyServices(toastMaker, settingsManager, keyboard, backgroundConnectionController, notificationService);
            //Start();
        }

        private async Task Start()
        {
            await _splashScreen.GetDB(_signInViewModel, _tMaker);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _splashScreen = new SplashScreen(this, _notificationService, _backgroundConnectionController);
            Start();
            return new Window(new NavigationPage(_splashScreen));
        }

        protected override void OnSleep()
        {
            AppData.Dispose();

            //AppData.AppState state = AppData.AppState.NormalConnected;
            switch (AppData.State)
            {
                case AppData.AppState.NormalConnected:
                    AppData.State = AppData.AppState.BackgroundConnected;
                    _backgroundConnectionController.Start();
                    break;
                case AppData.AppState.NormalDisconnected:
                    AppData.State = AppData.AppState.BackgroundDisconnected;
                    break;
            }
            base.OnSleep();
        }
        protected override void OnResume()
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
    }
}