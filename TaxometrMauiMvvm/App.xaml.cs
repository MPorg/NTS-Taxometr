using SQLitePCL;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Interfaces;
using TaxometrMauiMvvm.Platforms.Android.Services;
using TaxometrMauiMvvm.Views.Pages;

namespace TaxometrMauiMvvm
{
    public partial class App : Application
    {
        private IToastMaker _tMaker; 
        SplashScreen _splashScreen;
        public App(IToastMaker toastMaker, ISettingsManager settingsManager, IKeyboard keyboard)
        {
            InitializeComponent();
            _tMaker = toastMaker;
            AppData.SetDependencyServices(toastMaker, settingsManager, keyboard);
            //Start();
        }

        private async Task Start()
        {
            await _splashScreen.GetDB(_tMaker);
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            _splashScreen = new SplashScreen(this);
            Start();
            return new Window(_splashScreen);
        }
    }
}