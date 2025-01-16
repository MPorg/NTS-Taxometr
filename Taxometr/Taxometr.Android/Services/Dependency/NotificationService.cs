using Taxometr.Droid.Services.Dependency;
using Taxometr.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationService))]
namespace Taxometr.Droid.Services.Dependency
{
    internal class NotificationService : INotificationService
    {
        public int ShowNotification(string title, string message)
        {
            Notification.GetNotification(out int id, title, message, false, true);
            return id;
        }

        public void CloseNotifications()
        {
            Notification.CloseNotifications();
        }
    }
}