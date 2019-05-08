using MediatR;

namespace SurfLevel.Domain.Events
{
    public class PaymentSucceeded : INotification
    { 
        public string RequestID { get; set; }
    }
}
