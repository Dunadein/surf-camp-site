using SurfLevel.Contracts.Interfaces.Notificators;
using SurfLevel.Contracts.Models.Notificators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Notificators
{
    public abstract class Notificator<TNotification> : INotificator<TNotification> where TNotification : Notification
    {
        public abstract Task SendNotificationAsync(TNotification notification);

        protected string PrepareBody(string template, Dictionary<string, string> replacement)
        {
            var body = replacement.Aggregate(template, (current, value) =>
                current.Replace(value.Key, value.Value));

            return body;
        }
    }
}
