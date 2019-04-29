using MediatR;
using Microsoft.Extensions.Options;
using SurfLevel.Contracts.Interfaces.Notificators;
using SurfLevel.Contracts.Models.Notificators;
using SurfLevel.Domain.Events;
using SurfLevel.Domain.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Handlers
{
    public class AdminOrderCreatedHandler : INotificationHandler<CreatedOrder>
    {
        private readonly INotificator<AdminNotification> _notificator;
        private readonly SystemEmails _emailOptions;

        public AdminOrderCreatedHandler(IOptions<SystemEmails> options,
            INotificator<AdminNotification> notificator)
        {
            _notificator = notificator;
            _emailOptions = options.Value;
        }

        public async Task Handle(CreatedOrder booking, CancellationToken cancellationToken)
        {
            var notification = new AdminNotification()
            {
                Order = booking.Order,
                Recipient = _emailOptions.AdminEmails
            };

            await _notificator.SendNotificationAsync(notification);
        }
    }
}
