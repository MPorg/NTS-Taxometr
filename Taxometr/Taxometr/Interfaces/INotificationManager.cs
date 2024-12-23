using System.Collections.Generic;

namespace Taxometr.Interfaces
{
    public interface INotificationManager
    {
        void ShowNotification(string title, string message, IDictionary<string, string> data);
    }
}
