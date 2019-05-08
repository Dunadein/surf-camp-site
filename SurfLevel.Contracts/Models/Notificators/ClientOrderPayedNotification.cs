using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Contracts.Models.Notificators
{
    public class ClientOrderPayedNotification : Notification
    {
        public Order Order { get; set; }
    }
}
