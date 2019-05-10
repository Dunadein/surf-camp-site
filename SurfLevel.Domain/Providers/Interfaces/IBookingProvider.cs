using SurfLevel.Domain.ViewModels.Booking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers.Interfaces
{
    public interface IBookingProvider : IHashedProvider
    {
        Task<decimal> CalculateTotalPrice(IEnumerable<PickedService> services, string hash);

        Task<string> CreateBooking(BookingForm bookingForm);
    }
}
