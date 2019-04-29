using SurfLevel.Contracts.Models.DatabaseObjects;
using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.Notificators
{
    public class ClientNotification : Notification
    {
        public string Locale { get; set; }

        public IEnumerable<string> HiddenCopyTo { get; set; }

        public Order Order { get; set; }
    }
}
