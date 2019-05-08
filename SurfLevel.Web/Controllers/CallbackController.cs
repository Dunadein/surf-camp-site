using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Options;
using SurfLevel.Domain.Providers.Interfaces;
using System.Threading.Tasks;

namespace SurfLevel.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        private readonly CallbackOptions _options;
        private readonly IPaymentCallback _callback;

        public CallbackController(IOptions<CallbackOptions> options, IPaymentCallback paymentCallback)
        {
            _callback = paymentCallback;
            _options = options.Value;
        }

        [HttpGet("yandex-handler")]
        public IActionResult GetOrderParameters([FromQuery(Name = "cps_context_id")]string yandexRequestId,
            [FromQuery(Name = "id")]string orderHash)
            => Redirect(string.Format(_options.ProcessingPageTemplate, orderHash, yandexRequestId));

        [HttpGet("get-status/{id:string}")]
        public async Task<PaymentStatus> GetPaymentStatus([FromBody]string requestId)
            => await _callback.CheckPayment(requestId);
    }
}