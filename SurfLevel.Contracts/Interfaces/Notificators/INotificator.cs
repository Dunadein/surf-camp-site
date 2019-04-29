using SurfLevel.Contracts.Models.Notificators;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Notificators
{
    public interface INotificator<TNotification> where TNotification : Notification
    {
        Task SendNotificationAsync(TNotification notification);
    }
}
