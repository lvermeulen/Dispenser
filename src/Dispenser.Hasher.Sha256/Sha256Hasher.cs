using System.Security.Cryptography;

namespace Dispenser.Hasher.Sha256
{
    public class Sha256Hasher : HasherBase
    {
        protected override HashAlgorithm HashAlgorithm => SHA256.Create();
    }
}
