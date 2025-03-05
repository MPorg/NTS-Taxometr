using TaxometrMauiMvvm.Interfaces;

namespace TaxometrMauiMvvm.Services.Background;

public class NotificationService : INotificationService
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
