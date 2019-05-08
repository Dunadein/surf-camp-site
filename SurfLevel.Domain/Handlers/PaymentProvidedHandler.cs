using MediatR;
using SurfLevel.Contracts.Interfaces.Notificators;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.Notificators;
using SurfLevel.Domain.Events;
using SurfLevel.Domain.Extensions;
using System.Threading;
using System.Threading.Tasks;
using static SurfLevel.Domain.Fetching.PrimaryKeyStrategy;

namespace SurfLevel.Domain.Handlers
{
    public class PaymentProvidedHandler : INotificationHandler<PaymentProvided>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly INotificator<ClientOrderPayedNotification> _notificator;

        public PaymentProvidedHandler(IBookingRepository bookingRepository,
            INotificator<ClientOrderPayedNotification> notificator)
        {
            _bookingRepository = bookingRepository;
            _notificator = notificator;
        }

        public async Task Handle(PaymentProvided data, CancellationToken cancellationToken)
        {
            var order = await _bookingRepository.GetBookingByConditionAsync(GetById<Order>(data.OrderId));

            if ((order.Payed ?? 0) >= order.SurchargeAmount())
            {
                await _bookingRepository.UpdateOrderAsync(data.OrderId, p => p.Status = OrderStatus.Payed);

                var notification = new ClientOrderPayedNotification()
                {
                    Order = order,
                    Recipient = new string[] { order.ContactEmail }
                };

                await _notificator.SendNotificationAsync(notification);
            }
            else
            {
                await _bookingRepository.UpdateOrderAsync(data.OrderId, p => p.Status = OrderStatus.PartlyPayed);
            }
        }
    }
}
