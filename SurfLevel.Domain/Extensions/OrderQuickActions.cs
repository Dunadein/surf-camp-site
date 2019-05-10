using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Linq;

namespace SurfLevel.Domain.Extensions
{
    public static class OrderQuickActions
    {
        public static string GenerateName(this Order order)
        {
            return order.Id.ToString($"MAR{order.DateFrom.Month.ToString("00")}0000");
        }

        public static decimal SurchargeAmount(this Order order, Func<Service, bool> condition = null)
        {
            if (!order.IsCommission)
                return 0;

            return Math.Round(order.Services.Where(p => condition != null ? condition(p) : true)
                .Sum(p => p.Price * p.Package.Percent / 100) / 5.0m
            ) * 5;
        }
    }
}
