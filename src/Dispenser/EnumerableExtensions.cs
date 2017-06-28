using System.Collections.Generic;
using System.Linq;

namespace Dispenser
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<HashedPair<T>> Hash<T>(this IEnumerable<T> items, IHasher hasher) =>
            items.Select(x => new HashedPair<T>(x, hasher));
    }
}
