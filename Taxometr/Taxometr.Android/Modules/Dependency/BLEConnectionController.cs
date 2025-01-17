using Android.Content;
using Taxometr.Droid.Modules.Dependency;
using Taxometr.Interfaces;
[assembly: Xamarin.Forms.Dependency(typeof(BLEConnectionController))]

namespace Taxometr.Droid.Modules.Dependency
{
    public class BLEConnectionController : IBLEConnectionController
    {
        public void Start()
        {
            Intent intent = Intent;
            Android.App.Application.Context.StartService(intent);
        }

        public void Stop()
        {
            Intent intent = Intent;
            Android.App.Application.Context.StopService(intent);
        }

        private Intent Intent
        {
            get => new Intent().SetComponent(new ComponentName("com.nts.taxometr", "com.nts.taxometr.connection"));
        }
    }
}