using System.Collections.Generic;

namespace Dispenser
{
    public interface IDispenserSourceStorage<T>
    {
        IEnumerable<HashedPair<T>> Retrieve(string key);
        IEnumerable<HashedPair<T>> Store(string key, IEnumerable<T> values);
    }
}
