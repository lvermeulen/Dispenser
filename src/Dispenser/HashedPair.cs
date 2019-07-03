using System.Collections.Generic;
using System.Text;

namespace Dispenser
{
    public class HashedPair<T>
    {
        public HashedPair(T t, IHasher hasher, IEnumerable<string> excludePropertyNames = null, Encoding encoding = null)
        {
            Value = t;
            HashValue = hasher?.Hash(Value, excludePropertyNames, encoding);
        }

        public string HashValue { get; }
        public T Value { get; }
    }
}
