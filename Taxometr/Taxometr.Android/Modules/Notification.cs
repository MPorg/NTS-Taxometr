using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using System.Collections.Generic;
using AndroidApp = Android.App.Application;

namespace Taxometr.Droid.Modules
{
    public class Notification
    {
        private static string _foregroundChannelId = "9101";
        private static Context _context = global::Android.App.Application.Context;

        public Android.App.Notification GetNotification(string title, string message = "Нажмите, чтобы открыть приложение")
        {
            var intent = new Intent(_context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
                ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Mutable
                : PendingIntentFlags.UpdateCurrent;
            var pendingActivityIntent = PendingIntent.GetBroadcast(_context, 0, intent, pendingIntentFlags);
            var pendingIntent = PendingIntent.GetActivity(_context, 0, intent, pendingIntentFlags);

            var notifBuilder = new NotificationCompat.Builder(_context, _foregroundChannelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Mipmap.launcher_foreground)
                .SetOngoing(true)
                .SetContentIntent(pendingIntent);

            // Building channel if API verion is 26 or above
            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(_foregroundChannelId, "Системные сервисы", NotificationImportance.High);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                var notifManager = _context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notifManager != null)
                {
                    notifBuilder.SetChannelId(_foregroundChannelId);
                    notifManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notifBuilder.Build();
        }

        //------------------------------------------------------------------------//

        private const string CHANNEL_ID = "local_notifications_channel";
        private const string CHANNEL_NAME = "Уведомления";
        private const string CHANNEL_DESCRIPTION = "Уведомления приложения";

        private int notificationId = -1;
        private const string TITLE_KEY = "title";
        private const string MESSAGE_KEY = "message";

        private bool isChannelInitialized = false;

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, CHANNEL_NAME, NotificationImportance.High)
            {
                Description = CHANNEL_DESCRIPTION
            };
            channel.Importance = NotificationImportance.High;
            channel.EnableLights(true);
            channel.EnableVibration(true);
            channel.SetShowBadge(true);
            channel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });


            var notificationManager = _context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.CreateNotificationChannel(channel);
        }

        public void ShowNotification(string title, string message, IDictionary<string, string> data)
        {
            if (!isChannelInitialized)
            {
                CreateNotificationChannel();
            }

            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TITLE_KEY, title);
            intent.PutExtra(MESSAGE_KEY, message);
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            notificationId++;

            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, notificationId, intent, PendingIntentFlags.Mutable);
            var notificationBuilder = new NotificationCompat.Builder(AndroidApp.Context, CHANNEL_ID)
                                            .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Mipmap.icon))
                                            .SetSmallIcon(Resource.Mipmap.launcher_foreground)
                                            .SetContentTitle(title)
                                            .SetContentText(message)
                                            .SetAutoCancel(true)
                                            .SetContentIntent(pendingIntent)
                                            
                                            .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            var notificationManager = NotificationManagerCompat.From(AndroidApp.Context);

            notificationManager.Notify(notificationId, notificationBuilder.Build());
        }
    }
}