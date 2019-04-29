using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Domain.Extensions
{
    public static class OrderNameGenerator
    {
        public static string GenerateName(this Order order)
        {
            return order.Id.ToString($"MAR{order.DateFrom.Month.ToString("00")}0000");
        }
    }
}
