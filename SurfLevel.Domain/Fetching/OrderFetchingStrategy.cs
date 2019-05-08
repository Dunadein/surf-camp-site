using SurfLevel.Contracts.Models.DatabaseObjects;
using System;

namespace SurfLevel.Domain.Fetching
{
    internal static partial class FetchStrategy
    {
        public static Func<Order, bool> StartFrom(DateTime? startFrom) =>
            p => startFrom.HasValue ? p.DateFrom >= startFrom.Value : true;

        public static Func<Order, bool> InPeriod(DateTime periodStart, DateTime? periodEnd = null) =>
            p => p.DateTill >= periodStart && (periodEnd.HasValue ? p.DateFrom <= periodEnd.Value : true);
    }
}
