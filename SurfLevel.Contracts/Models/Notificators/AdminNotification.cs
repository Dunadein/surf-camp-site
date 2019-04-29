using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Contracts.Models.Notificators
{
    public class AdminNotification : Notification
    {
        public Order Order { get; set; }
    }
}
