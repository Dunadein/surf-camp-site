using System.Collections.Generic;
using System.Linq;

namespace SurfLevel.Domain.Extensions
{
    public static class SqlLikeFunctions
    {
        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        public static bool In<T>(this T value, params T[] items)
        {
            return items.Contains(value);
        }
    }
}
