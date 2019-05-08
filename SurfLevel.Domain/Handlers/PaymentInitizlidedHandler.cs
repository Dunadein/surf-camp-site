using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Domain.Events;

namespace SurfLevel.Domain.Handlers
{
    public class PaymentInitizlidedHandler : INotificationHandler<PaymentInitialization>
    {
        private readonly IPaymentRepository _repository;

        public PaymentInitizlidedHandler(IPaymentRepository paymentRepository)
        {
            _repository = paymentRepository;
        }

        public async Task Handle(PaymentInitialization data, CancellationToken cancellationToken)
        {
            var row = new PayLog()
            {
                Amount = data.Amount,
                FullAmount = data.AmountToPay,
                Direction = Direction.Out,
                OrderId = data.OrderId,
                RequestId = data.RequestId,
                Time = DateTime.UtcNow,
                Status = Status.Open,
                EuroAmount = data.EuroAmount
            };

            await _repository.SaveRowAsync(row);
        }
    }
}
