using MediatR;
using Microsoft.Extensions.Options;
using SurfLevel.Contracts.Interfaces.Notificators;
using SurfLevel.Contracts.Models.Notificators;
using SurfLevel.Domain.Events;
using SurfLevel.Domain.Options;
using System.Threading;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Handlers
{
    public class OrderCreatedHandler : INotificationHandler<CreatedOrder>
    {
        private readonly INotificator<ClientNewOrderNotification> _notificator;
        private readonly SystemEmails _emailOptions;

        public OrderCreatedHandler(IOptions<SystemEmails> options,
            INotificator<ClientNewOrderNotification> notificator)
        {
            _notificator = notificator;
            _emailOptions = options.Value;
        }

        public async Task Handle(CreatedOrder booking, CancellationToken cancellationToken)
        {
            var notification = new ClientNewOrderNotification()
            {
                Order = booking.Order,
                Recipient = new string[] { booking.Order.ContactEmail },
                HiddenCopyTo = new string [] { _emailOptions.SystemEmail }
            };

            await _notificator.SendNotificationAsync(notification);
        }
    }
}
