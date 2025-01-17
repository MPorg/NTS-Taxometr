using Taxometr.Data;
using Taxometr.Interfaces;
using Xamarin.Forms;

namespace Taxometr
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (AppData.MainMenu == null)
            {
                MainMenu mm = new MainMenu();
                MainPage = mm;
                AppData.MainMenu = mm;
                AppData.Initialize();
            }
            else
            {
                MainPage = AppData.MainMenu;
                AppData.Initialize();
                DependencyService.Resolve<INotificationService>().CloseNotifications();
                switch (AppData.State)
                {
                    case AppData.AppState.BackgroundConnected:
                        AppData.State = AppData.AppState.NormalConnected;
                        DependencyService.Resolve<IBLEConnectionController>().Stop();
                        break;
                    case AppData.AppState.BackgroundDisconnected:
                        AppData.State = AppData.AppState.NormalDisconnected;
                        break;
                }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            AppData.Dispose();
            switch (AppData.State)
            {
                case AppData.AppState.NormalConnected:
                    AppData.State = AppData.AppState.BackgroundConnected;
                    DependencyService.Resolve<IBLEConnectionController>().Start();
                    break;
                case AppData.AppState.NormalDisconnected:
                    AppData.State = AppData.AppState.BackgroundDisconnected;
                    break;
            }
        }

        protected override void OnResume()
        {
            DependencyService.Resolve<INotificationService>().CloseNotifications();
            switch (AppData.State)
            {
                case AppData.AppState.BackgroundConnected:
                    AppData.State = AppData.AppState.NormalConnected;
                    DependencyService.Resolve<IBLEConnectionController>().Stop();
                    break;
                case AppData.AppState.BackgroundDisconnected:
                    AppData.State = AppData.AppState.NormalDisconnected;
                    break;
            }
        }
    }
}
