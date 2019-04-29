using MediatR;
using Microsoft.Extensions.Options;
using SurfLevel.Contracts.Interfaces.Notificators;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.Notificators;
using SurfLevel.Domain.Events;
using SurfLevel.Domain.Options;
using System.Threading;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Handlers
{
    public class OrderCreatedHandler : INotificationHandler<CreatedOrder>
    {
        private readonly INotificator<ClientNotification> _notificator;
        private readonly ILocaleService _locale;
        private readonly SystemEmails _emailOptions;

        public OrderCreatedHandler(IOptions<SystemEmails> options,
            INotificator<ClientNotification> notificator,
            ILocaleService langService)
        {
            _notificator = notificator;
            _locale = langService;
            _emailOptions = options.Value;
        }

        public async Task Handle(CreatedOrder booking, CancellationToken cancellationToken)
        {
            var notification = new ClientNotification()
            {
                Order = booking.Order,
                Locale = _locale.GetUserLocale(),
                Recipient = new string[] { booking.Order.ContactEmail },
                HiddenCopyTo = new string [] { _emailOptions.SystemEmail }
            };

            await _notificator.SendNotificationAsync(notification);
        }
    }
}
