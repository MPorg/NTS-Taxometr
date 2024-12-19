using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gestures;
using Android.OS;
using System;
using System.Threading.Tasks;

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

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}