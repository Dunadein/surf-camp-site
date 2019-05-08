using Microsoft.AspNetCore.Mvc;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Order;
using System.Threading.Tasks;

namespace SurfLevel.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderProvider _provider;

        public OrderController(IOrderProvider orderProvider)
        {
            _provider = orderProvider;
        }

        [HttpGet("get-order/{id:string}")]
        public async Task<ViewOrder> GetOrder(string id)
            => await _provider.GetOrder(id);

        [HttpPost("make-prepayment/{id:string}")]
        public async Task<IActionResult> RedirectToPaymentProvider(string id, [FromForm]PrepayType type, [FromForm]int? serviceId = null)
            => Redirect(await _provider.GetPaymentUrl(id, type, serviceId));

        [HttpPost("save-guest/{id:string}")]
        public async Task SaveGuest(string id, [FromForm]int guestNumber, [FromForm] string name, [FromForm]string lastname)
            => await _provider.UpdateGuest(id, guestNumber, name, lastname);
    }
}