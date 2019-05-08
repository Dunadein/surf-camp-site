using MediatR;

namespace SurfLevel.Domain.Events
{
    public class PaymentInitialization : INotification
    {
        public int OrderId { get; set; }
        public decimal EuroAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountToPay { get; set; }
        public string RequestId { get; set; }
    }
}
