using Android.App;
using Android.Content;
using Android.OS;
using TaxometrMauiMvvm.Data;
using TaxometrMauiMvvm.Data.DataBase.Objects;
using static Android.Provider.ContactsContract.CommonDataKinds;

namespace TaxometrMauiMvvm.Services.Background;

[Service (Name = "com.nts.taxometr.ConnectionWorker", ForegroundServiceType = Android.Content.PM.ForegroundService.TypeManifest, Exported = true, Enabled = true)]
public class ConnectionWorker : Service
{
    public override IBinder OnBind(Intent intent)
    {
        return null;
    }

    public const int ServiceRunningNotifID = 9000;

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        DevicePrefab prefab = AppData.ConnectedDP;
        string name = "No name";
        if (prefab != null) name = prefab.CustomName;
        Android.App.Notification notif = Notification.GetNotification(out int id, $"Подключено устройство {name}");
        StartForeground(ServiceRunningNotifID, notif);

        return StartCommandResult.Sticky;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
