using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispenser
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<HashedPair<T>> Hash<T>(this IEnumerable<T> items, IHasher hasher, IEnumerable<string> excludePropertyNames = null, Encoding encoding = null) =>
            items.Select(x => new HashedPair<T>(x, hasher, excludePropertyNames, encoding));
    }
}
