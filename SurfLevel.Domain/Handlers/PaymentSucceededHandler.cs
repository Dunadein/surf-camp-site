using MediatR;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Handlers
{
    public class PaymentSucceededHandler : INotificationHandler<PaymentSucceeded>
    {
        private readonly IPaymentRepository _repository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMediator _bus;

        public PaymentSucceededHandler(IPaymentRepository paymentRepository,
            IBookingRepository bookingRepository,
            IMediator bus)
        {
            _repository = paymentRepository;
            _bookingRepository = bookingRepository;
            _bus = bus;
        }

        public async Task Handle(PaymentSucceeded payment, CancellationToken cancellationToken)
        {
            var outRow = await _repository.GetRowByConditionAsync(p => 
                p.RequestId == payment.RequestID && p.Status == Status.Open);

            if (outRow != null)
            {
                var inRow = new PayLog()
                {
                    Direction = Direction.In,
                    Amount = outRow.Amount,
                    EuroAmount = outRow.EuroAmount,
                    OrderId = outRow.OrderId,
                    RequestId = outRow.RequestId,
                    Status = Status.Closed,
                    Time = DateTime.UtcNow
                };

                await _repository.SaveRowAsync(inRow);

                await _repository.UpdateRow(outRow.Id, p => p.Status = Status.Closed);

                await _bookingRepository.UpdateOrderAsync(outRow.OrderId, p => p.Payed += outRow.EuroAmount);

                await _bus.Publish(new PaymentProvided() { OrderId = outRow.OrderId, Amount = outRow.Amount });
            }
        }
    }
}
