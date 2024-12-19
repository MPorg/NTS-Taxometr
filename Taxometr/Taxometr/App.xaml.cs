using Taxometr.Data;
using Xamarin.Forms;

namespace Taxometr
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainMenu mm = new MainMenu();
            MainPage = mm;
            AppData.MainMenu = mm;
            AppData.Initialize();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            AppData.Dispose();
        }

        protected override void OnResume()
        {
        }

        
    }
}
