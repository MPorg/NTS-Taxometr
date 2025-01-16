using Android.Content;
using Taxometr.Droid.Services.Dependency;
using Taxometr.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(BLEConnectionController))]
namespace Taxometr.Droid.Services.Dependency
{
    public class BLEConnectionController : IBLEConnectionController
    {
        public void Start()
        {
            Intent intent = CreateIntent();
            Android.App.Application.Context.StartService(intent);
        }

        public void Stop()
        {
            Intent intent = CreateIntent();
            Android.App.Application.Context.StopService(intent);
        }

        private Intent CreateIntent()
        {
            return new Intent().SetComponent(new ComponentName("com.nts.taxometr", "com.nts.taxometr.connection_worker"));
        }
    }
}