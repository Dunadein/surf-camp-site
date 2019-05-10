using Microsoft.AspNetCore.Mvc;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Booking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingProvider _provider;

        public BookingController(IBookingProvider bookingProvider)
        {
            _provider = bookingProvider;
        }

        [HttpGet("get-price/{hash}")]
        public async Task<decimal> GetTotalPrice(string hash, [FromBody]IEnumerable<PickedService> services)
            => await _provider.CalculateTotalPrice(services, hash);

        [HttpPost("create-booking")]
        public async Task<string> CreateNewOrder([FromBody]BookingForm bookingForm)
            => await _provider.CreateBooking(bookingForm);
    }
}