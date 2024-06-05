using Plugin.BluetoothClassic.Abstractions;
using System.Diagnostics;
using Taxometr.DataBase;
using Taxometr.DataBase.Options;
using Taxometr.DataBase.Tmp;
using Xamarin.Forms;

namespace Taxometr
{
    public partial class App : Application
    {
        private static TaxometerLocalDB taxometerLocalDB;
        public static TaxometerLocalDB TaxometerLocalDB
        {
            get
            {
                if(taxometerLocalDB == null)
                {
                    taxometerLocalDB = new TaxometerLocalDB(DB.DatabasePath);
                }
                return taxometerLocalDB;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            AppData.Adapter = DependencyService.Resolve<IBluetoothAdapter>();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
