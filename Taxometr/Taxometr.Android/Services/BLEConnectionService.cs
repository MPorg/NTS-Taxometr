using Android.App;
using Android.Content;
using Android.OS;
using Taxometr.Data;

namespace Taxometr.Droid.Services
{
    [Service(Name = "com.nts.taxometr.connection")]
    public class BLEConnectionService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public const int ServiceStopNotifID = 9099;
        public const int ServiceRunningNotifID = 9100;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Notification notif = new Modules.Notification().GetNotification($"Подключено {AppData.ConnectedDevicePrefab().Result.CustomName}");
            StartForeground(ServiceRunningNotifID, notif);
            
            AppData.OnBG = true;
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            AppData.OnBG = false;
            base.OnDestroy();
        }
    }
}