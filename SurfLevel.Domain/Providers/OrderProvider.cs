using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Domain.Extensions;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers
{
    public class OrderProvider : IOrderProvider
    {
        private readonly IHasherService<string> _hasher;
        private readonly IBookingRepository _repository;
        private readonly IPaymentService _paymentService;

        public OrderProvider(IHasherService<string> orderHasher,
            IBookingRepository bookingRepository,
            IPaymentService paymentService)
        {
            _hasher = orderHasher;
            _repository = bookingRepository;
            _paymentService = paymentService;
        }

        private async Task<Order> GetOrderByHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentNullException($"{nameof(hash)} wasn't provided.");

            var id = _hasher.Read(hash);

            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("The given Id cannot be readed.");

            var order = await _repository.GetBookingByConditionAsync(p => p.HashKey == id);

            if (order == null)
                throw new ArgumentNullException("There is no order with such Id");

            return order;
        }

        public async Task<string> GetPaymentUrl(string hash, PrepayType type, int? serviceId = null)
        {
            var order = await GetOrderByHash(hash);

            var amount = GetAmount(type, order, serviceId);

            return await _paymentService.GetPaymentURL(order.Id, order.GenerateName(), amount, hash);
        }

        private decimal GetAmount(PrepayType type, Order order, int? serviceId)
        {
            switch (type)
            {
                case PrepayType.FullPayment:
                    return order.SurchargeAmount();
                case PrepayType.PerService:
                {
                    if (!serviceId.HasValue)
                        throw new ArgumentNullException("Service not chosen.");

                    return order.SurchargeAmount(p => order.Services.ToList()[serviceId.Value - 1].Id == p.Id);
                }
                case PrepayType.PerPerson:
                    return order.SurchargeAmount() / order.GuestsCount;
                default:
                    throw new NotImplementedException("Unknown payment type.");
            }
        }        

        public async Task UpdateGuest(string hash, int number, string name, string secondName)
        {
            var order = await GetOrderByHash(hash);

            if (number > order.GuestsCount)
                throw new ArgumentException("There is no guest with such a number.");

            var guest = order.Guests.ToList()[number - 1];

            await _repository.UpdateTouristAsync(guest.Id, name, secondName);
        }

        public async Task<ViewOrder> GetOrder(string hash)
        {
            var order = await GetOrderByHash(hash);

            var viewOrder = new ViewOrder()
            {
                OrderId = order.GenerateName(),
                Email = order.ContactEmail,
                From = order.DateFrom.ToShortDateString(),
                Till = order.DateTill?.ToShortDateString(),
                FullName = $"{order.GuestName} {order.GuestSecondName}",
                Pax = order.GuestsCount,
                Phone = order.ContactPhone,
                FullPrice = order.TotalPrice,
                Status = order.Status,
                Guests = order.Guests.Select((p, i) => new ViewGuest()
                {
                    Number = i + 1,
                    Name = p.Name,
                    SecondName = p.LastName
                }),
                Services = order.Services.Select((p, i) => new ViewService()
                {
                    ServiceKey = i + 1,
                    PackageId = p.PackageId,
                    AccommodationPriceId = p.AccommodationPriceId
                })
            };

            if (order.Services.Any(p => p.PrepayPercent.HasValue) && order.Status != OrderStatus.Payed)
            {                
                viewOrder.Prepayment = await GetPrepaymentInfo(order);
            }

            return viewOrder;
        }

        private async Task<PrepaymentInfo> GetPrepaymentInfo(Order order)
        {
            var types = new List<PrepayType>();

            var prepayAmount = order.SurchargeAmount() - (order.Payed ?? 0);

            var amountToPay = await _paymentService.GetConvertedAmount(prepayAmount);

            if (_paymentService.IsOperableAmount(amountToPay) && order.Status != OrderStatus.PartlyPayed)
                types.Add(PrepayType.FullPayment);

            if (order.Services.Count > 1)
                types.Add(PrepayType.PerService);

            if (order.GuestsCount > 1 && order.Services.Count == 1)
                types.Add(PrepayType.PerPerson);

            return new PrepaymentInfo()
            {
                AvailableType = types,
                Payed = order.Payed ?? 0,
                Amount = $"{prepayAmount}EUR (~{amountToPay}руб.)"
            };
        }
    }
}
