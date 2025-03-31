using Notif = Android.App.Notification;
using static TaxometrMauiMvvm.Services.Background.ButtonExtras;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Resource = Microsoft.Maui.Resource;
using MAUIApp = Microsoft.Maui.Controls.Application;

namespace TaxometrMauiMvvm.Services.Background;

public static class Notification
{
    private static string foregroundChannelId = "9001";
    private static Context context = global::Android.App.Application.Context;
    private static List<Notif> notifications = new List<Notif>();

    public static Notif GetNotification(out int Id, string title, string text = "Нажмите, чтобы открыть приложение", bool hasBtn = true, bool andShow = false)
    {
        var intent = new Intent(context, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.SingleTop);
        intent.PutExtra("Title", "Message");

        var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            ? PendingIntentFlags.CancelCurrent | PendingIntentFlags.Mutable
            : PendingIntentFlags.CancelCurrent;
        var pendingActivityIntent = PendingIntent.GetBroadcast(context, 0, intent, pendingIntentFlags);
        var pendingIntent = PendingIntent.GetActivity(context, 0, intent, pendingIntentFlags);

        int color = 0;

        if (MAUIApp.Current.Resources.TryGetValue("BackgroundDarked", out var colorStr))
        {
            if (colorStr is Color color1)
            {
                color = color1.ToInt();
            }
        }

        var notifBuilder = new NotificationCompat.Builder(context, foregroundChannelId)
            .SetContentTitle(title)
            .SetContentText(text)
            .SetColor(color)
            .SetColorized(true)
            .SetSmallIcon(Resource.Drawable.notif_icon)
            .SetStyle(new NotificationCompat.DecoratedCustomViewStyle())
            .SetAutoCancel(false)
            .SetContentIntent(pendingIntent);

        if (hasBtn)
        {
            var actionIntent = new Intent(context, typeof(NotificationButtonReciver));
            actionIntent.PutExtra(CancelBtn, CancelBtnValue);
            var broadcastPendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable
            : PendingIntentFlags.UpdateCurrent;
            var actionPendingIntent = PendingIntent.GetBroadcast(context, 0, actionIntent, broadcastPendingIntentFlags);
            notifBuilder.SetOngoing(true);
            notifBuilder.AddAction(1, "Отключить", actionPendingIntent);
        }

        // Building channel if API verion is 26 or above
        if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            NotificationChannel notificationChannel = new NotificationChannel(foregroundChannelId, "Уведомления", NotificationImportance.High);
            notificationChannel.Importance = NotificationImportance.High;
            notificationChannel.EnableLights(true);
            notificationChannel.EnableVibration(true);
            notificationChannel.SetShowBadge(true);
            notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

            var notifManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            if (notifManager != null)
            {
                notifBuilder.SetChannelId(foregroundChannelId);
                notifManager.CreateNotificationChannel(notificationChannel);
            }
        }

        var notif = notifBuilder.Build();
        notifications.Add(notif);

        Id = notifications.IndexOf(notif);

        if (andShow)
        {
            ShowNotification(notif);
        }
        return notif;
    }

    public static void ShowNotification(Notif notification)
    {
        var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);
        notificationManager.Notify(int.Parse(foregroundChannelId), notification);
    }

    public static void CloseNotifications()
    {
        var notificationManager = NotificationManagerCompat.From(Android.App.Application.Context);
        notificationManager.CancelAll();
    }
}
