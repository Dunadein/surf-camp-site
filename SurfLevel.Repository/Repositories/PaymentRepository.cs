using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.DBProviders;
using System;
using System.Threading.Tasks;

namespace SurfLevel.Repository.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentContext _context;

        public PaymentRepository(PaymentContext context)
        {
            _context = context;
        }

        public async Task SaveRowAsync(PayLog row)
        {
            _context.PayLogs.Add(row);

            await _context.SaveChangesAsync();
        }

        public async Task<PayLog> GetRowByConditionAsync(Func<PayLog, bool> condition)
        {
            return await _context.PayLogs.AsNoTracking().FirstOrDefaultAsync(p => condition(p));
        }

        public async Task UpdateRow(int id, Action<PayLog> action)
        {
            var row = await _context.PayLogs.FindAsync(id);

            action(row);

            await _context.SaveChangesAsync();
        }
    }
}
