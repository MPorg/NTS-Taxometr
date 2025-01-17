namespace Taxometr.Interfaces
{
    public interface INotificationService
    {
        int ShowNotification(string title, string message);
        void CloseNotifications();
    }
}
