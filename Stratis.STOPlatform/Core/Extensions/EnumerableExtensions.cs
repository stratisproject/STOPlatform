using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T item, int i)> AsIndexed<T>(this IEnumerable<T> items)
        {
            return items.Select((item, i) => (item, i));
        }

        public static ulong Sum<T>(this IEnumerable<T> items, Func<T,ulong> property)
        {
            var total = 0ul;
            foreach (var item in items)
                total += property(item);

            return total;
        }
    }
}
