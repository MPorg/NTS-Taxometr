using Android.App;
using Android.Content;
using Android.OS;
using Taxometr.Data;

namespace Taxometr.Droid.Services
{
    [Service(Name = "com.nts.taxometr.connection_worker")]
    public class ConnectionWorker : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public const int ServiceRunningNotifID = 9000;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Android.App.Notification notif = Notification.GetNotification(out int id, $"Подключено устройство {AppData.ConnectedDP.CustomName}");
            StartForeground(ServiceRunningNotifID, notif);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}