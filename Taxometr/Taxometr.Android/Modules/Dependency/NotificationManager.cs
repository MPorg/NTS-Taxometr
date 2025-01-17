using System.Collections.Generic;
using Taxometr.Interfaces;
using Xamarin.Forms;
using NotificationManager = Taxometr.Droid.Modules.Dependency.NotificationManager;

[assembly: Dependency(typeof(NotificationManager))]
namespace Taxometr.Droid.Modules.Dependency
{
    public class NotificationManager : INotificationManager
    {
        public void ShowNotification(string title, string message, IDictionary<string, string> data)
        {
            new Notification().ShowNotification(title, message, data);
        }
    }
}