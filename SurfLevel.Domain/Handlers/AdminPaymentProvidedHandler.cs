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
    public class AdminPaymentProvidedHandler : INotificationHandler<PaymentProvided>
    {
        private readonly SystemEmails _options;
        private readonly INotificator<AdminOrderPayedNotification> _notificator;

        public AdminPaymentProvidedHandler(IOptions<SystemEmails> options, INotificator<AdminOrderPayedNotification> notificator)
        {
            _options = options.Value;
            _notificator = notificator;
        }

        public async Task Handle(PaymentProvided data, CancellationToken cancellationToken)
        {
            await _notificator.SendNotificationAsync(new AdminOrderPayedNotification()
            {
                Amount = data.Amount,
                OrderId = data.OrderId,
                Recipient = new string[] { _options.PaymentHandleEmail }
            });
        }
    }
}
