using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Providers.Interfaces;
using System;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers
{
    public class PaymentCallback : IPaymentCallback
    {        
        private readonly IPaymentService _paymentService;

        public PaymentCallback(IPaymentService paymentService)
        {           
            _paymentService = paymentService;
        }

        public async Task<PaymentStatus> CheckPayment(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
                throw new ArgumentNullException($"{nameof(requestId)} are not defined.");

            return await _paymentService.GetPaymentStatus(requestId);
        }
    }
}
