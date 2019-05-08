using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.DBProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Repository.Repositories
{
    internal static partial class Extension
    {
        public static IIncludableQueryable<Order, Accommodation> EndChain(this IQueryable<Order> chain)
        {
            return chain.Include(p => p.Guests).Include(p => p.Services).ThenInclude(p => p.Package)
                .Include(p => p.Services).ThenInclude(p => p.AccommodationPrice).ThenInclude(p => p.Accommodation);
        }
    }

    public class BookingRepository : IBookingRepository
    {
        private readonly BookingContext _context;

        public BookingRepository(BookingContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetBookingsByConditionAsync(Func<Order, bool> condition = null)
        {
            return await _context.Orders.Where(p => condition != null ? condition(p) : true)
                .EndChain().AsNoTracking().ToListAsync();
        }

        public async Task<Order> GetBookingByConditionAsync(Func<Order, bool> condition)
        {
            return await _context.Orders.EndChain().AsNoTracking()
                .FirstOrDefaultAsync(p => condition(p));
        }

        public async Task<int> CreateBookingAsync(Order order)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }

        public async Task UpdateTouristAsync(int id, string name, string secondName)
        {
            var row = await _context.Tourists.FindAsync(id);

            row.Name = name;
            row.LastName = secondName;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(int id, Action<Order> action)
        {
            var order = await _context.Orders.FindAsync(id);

            action(order);

            await _context.SaveChangesAsync();
        }
    }
}
