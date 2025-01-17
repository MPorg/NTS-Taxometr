using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Threading.Tasks;
using Taxometr.Data;
using Taxometr.Interfaces;
using Xamarin.Forms;

namespace Taxometr.Droid
{
    [Activity(Label = "НТС-Таксометр", Theme = "@style/SplashTheme",
        MainLauncher = true, NoHistory = true,
        ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Intent.Extras != null && Intent.Extras.ContainsKey("buttonClicked"))
            {
                AppData.Debug.WriteLine("______________Button clicked______________");

                DependencyService.Resolve<IBLEConnectionController>().Stop();
                FinishAndRemoveTask();

                return;
            }

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task simStartup = new Task(() => { SimulateStartupWork(); });
            simStartup.Start();
        }

        private async Task SimulateStartupWork()
        {
            await Task.Delay(500);

            StartActivity(new Intent(Android.App.Application.Context, typeof(MainActivity)));
        }
    }
}