using System.Collections.Generic;
using System.Text;

namespace Dispenser
{
    public interface IHasher
    {
        string Hash(object obj, IEnumerable<string> excludePropertyNames = null, Encoding encoding = null);
    }
}
