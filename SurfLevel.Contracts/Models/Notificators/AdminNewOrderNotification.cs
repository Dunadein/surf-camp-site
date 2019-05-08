using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Contracts.Models.Notificators
{
    public class AdminNewOrderNotification : Notification
    {
        public Order Order { get; set; }
    }
}
