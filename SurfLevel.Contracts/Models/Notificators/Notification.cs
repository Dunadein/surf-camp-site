using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.Notificators
{
    public abstract class Notification
    {
        public IEnumerable<string> Recipient { get; set; }
    }
}
