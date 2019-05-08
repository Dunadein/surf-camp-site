using MediatR;

namespace SurfLevel.Domain.Events
{
    public class PaymentProvided : INotification
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
