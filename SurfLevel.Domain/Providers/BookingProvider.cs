using MediatR;
using Microsoft.Extensions.Options;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Events;
using SurfLevel.Domain.Options;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Booking;
using SurfLevel.Domain.ViewModels.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SurfLevel.Domain.Fetching.PrimaryKeyStrategy;

namespace SurfLevel.Domain.Providers
{
    public class BookingProvider : IBookingProvider
    {
        private readonly IBookingRepository _booking;
        private readonly IPackageRepository _packages;
        private readonly IHasherService<SearchRequest> _searchHasher;
        private readonly IPricingService _pricing;
        private readonly IHasherService<string> _orderHasher;
        private readonly IMediator _bus;
        private readonly ILocaleService _localeService;
        private readonly CommissionLocales _comissionLocales;

        public BookingProvider(IBookingRepository bookingRepository,
            IPackageRepository packageRepository,
            IHasherService<SearchRequest> searchHasher,
            IPricingService pricingService,
            IHasherService<string> orderHasher,
            IMediator mediator,
            ILocaleService localeService,
            IOptions<CommissionLocales> options)
        {
            _packages = packageRepository;
            _searchHasher = searchHasher;
            _pricing = pricingService;
            _booking = bookingRepository;
            _orderHasher = orderHasher;
            _bus = mediator;
            _localeService = localeService;
            _comissionLocales = options.Value;
        }

        public async Task<decimal> CalculateTotalPrice(IEnumerable<PickedService> services, string hash)
        {
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentNullException("Searching parameters wasn't supplied.");

            var request = _searchHasher.Read(hash);

            var group = services.GroupBy(p => p.PackageId);

            var prices = await Task.WhenAll(group.Select(p => CalculatePackagePrice(request, p.Key, p)));

            return prices.Sum(price => price);
        }

        private async Task<decimal> CalculatePackagePrice(Request request, int packageId, IEnumerable<PickedService> service)
        {
            var package = await _packages.GetPackageByConditionAsync(GetById<Package>(packageId));

            if (package == null)
                throw new ArgumentNullException("The chosen package doesn't found");

            if (package.IsWithAccommodation && !service.Any(p => !p.RoomId.HasValue))
                throw new ArgumentException($"The package {package.Name} is going with accommodation, but the accommodation key wasn't provide.");

            var prices = await Task.WhenAll(service.Select(p =>
                _pricing.CalculateRequestedPriceAsync(package, p.RoomId, p.Pax, request.From, request.Till)
            ));

            return prices.Sum(p => p.Item2);
        }

        public async Task<string> CreateBooking(BookingForm bookingForm)
        {
            bookingForm.Validate();

            var locale = _localeService.GetUserLocale();

            var commission = _comissionLocales?.CommissionLocalesList.Contains(locale) ?? false;

            var packages = await _packages.GetPackagesByConditionAsync(p =>
                bookingForm.Services.Select(s => s.PackageId).Distinct().Contains(p.Id));

            var order = new Order()
            {
                HashKey = Guid.NewGuid().ToString(),
                Status = OrderStatus.Confirmed,
                DateFrom = bookingForm.From,
                DateTill = bookingForm.Till,
                GuestsCount = bookingForm.Pax,
                Created = DateTime.Now,
                Comment = bookingForm.Comment,
                ContactEmail = bookingForm.Email,
                ContactPhone = bookingForm.Phone,
                GuestName = bookingForm.Name,
                GuestSecondName = bookingForm.SecondName,
                Locale = locale,
                IsCommission = commission,
                Guests = Enumerable.Range(1, bookingForm.Pax).Select(p => new Guest()).ToArray()
            };

            foreach (var service in bookingForm.Services)
            {
                var package = packages.FirstOrDefault(p => p.Id == service.PackageId);

                var price = await _pricing.CalculateRequestedPriceAsync(package, service.RoomId, service.Pax, bookingForm.From, bookingForm.Till);

                order.Services.Add(new Service()
                {
                    PackageId = package.Id,
                    AccommodationPriceId = price.Item1,
                    ServiceDays = price.Item1.HasValue? (int?)null : service.Pax,
                    Price = price.Item2
                });
            }

            order.TotalPrice = order.Services.Sum(p => p.Price);

            var id = await _booking.CreateBookingAsync(order);

            if (id == 0)
                throw new Exception("The booking attempt failed.");

            await _bus.Publish(new CreatedOrder { Order = order });

            return _orderHasher.Create(order.HashKey);
        }
    }   
}
