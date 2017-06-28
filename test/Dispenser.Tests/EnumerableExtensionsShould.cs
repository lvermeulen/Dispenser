using System.Linq;
using Dispenser.Hasher.Sha1;
using Xunit;

namespace Dispenser.Tests
{
    public class EnumerableExtensionsShould
    {
        private readonly IHasher _hasher;

        public EnumerableExtensionsShould()
        {
            _hasher = new Sha1Hasher();
        }

        [Fact]
        public void Hash()
        {
            var value1 = new TypedValue<int>(1);
            var value2 = new TypedValue<int>(2);
            var value3 = new TypedValue<int>(3);

            var items = new[] { value1, value2, value3 };
            var expectedHashedItems = new[]
            {
                new HashedPair<TypedValue<int>>(value1, _hasher),
                new HashedPair<TypedValue<int>>(value2, _hasher),
                new HashedPair<TypedValue<int>>(value3, _hasher)
            }.ToList();
            var hashedItems = items.Hash(_hasher).ToList();

            Assert.NotNull(hashedItems);
            Assert.Equal(expectedHashedItems.Select(x => x.HashValue), hashedItems.Select(x => x.HashValue));
            Assert.Equal(expectedHashedItems.Select(x => x.Value), hashedItems.Select(x => x.Value));
        }
    }
}
