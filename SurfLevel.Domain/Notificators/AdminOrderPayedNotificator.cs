using SurfLevel.Contracts.Models.Notificators;
using SurfLevel.Contracts.Notificators;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Notificators
{
    public class AdminOrderPayedNotificator : Notificator<AdminOrderPayedNotification>
    {
        private readonly IMailNotification _mailer;

        public AdminOrderPayedNotificator(IMailNotification mailNotification)
        {
            _mailer = mailNotification;
        }

        public override async Task SendNotificationAsync(AdminOrderPayedNotification notification)
        {
            var body = $"По брони номер {notification.OrderId} учтена оплата в размере {notification.Amount} рублей.";

            await _mailer.SendMailAsync(notification.Recipient, $"Оплата брони #{notification.OrderId}", body);
        }
    }
}
