using SurfLevel.Contracts.Models.DatabaseObjects;
using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.Notificators
{
    public class ClientNewOrderNotification : Notification
    {
        public IEnumerable<string> HiddenCopyTo { get; set; }

        public Order Order { get; set; }
    }
}
