using System;
using System.Linq;
using Dispenser.Hasher.Sha1;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration

namespace Dispenser.Tests
{
    public class DispenserShould
    {
        private readonly StockItem[] _previousStock = 
        {
            new StockItem("Lumber 2x2", 7),
            new StockItem("Lumber 2x4", 5),
            new StockItem("Lumber 2x8", 4),
            new StockItem("Lumber 2x10", 2)
        };

        private readonly StockItem[] _actualStock =
        {
            new StockItem("Lumber 2x2", 3),
            new StockItem("Lumber 2x3", 6),
            new StockItem("Lumber 2x4", 3),
            new StockItem("Lumber 2x6", 3),
            new StockItem("Lumber 2x8", 4)
        };

        [Fact]
        public void CheckArguments()
        {
            var hasher = new Sha1Hasher();
            var values = _previousStock.Hash(hasher);

            Assert.Throws<ArgumentNullException>(() => new Dispenser<StockItem, string>().Dispense(null, values, x => x.Sku));
            Assert.Throws<ArgumentNullException>(() => new Dispenser<StockItem, string>().Dispense(values, null, x => x.Sku));
            Assert.Throws<ArgumentNullException>(() => new Dispenser<StockItem, string>().Dispense(values, values, null));
        }

        [Fact]
        public void DetectChanges()
        {
            var hasher = new Sha1Hasher();
            var results = new Dispenser<StockItem, string>().Dispense(_actualStock.Hash(hasher), _previousStock.Hash(hasher), x => x.Sku);

            Assert.NotNull(results);
            Assert.True(results.HasChanges);
            Assert.Contains(new StockItem("Lumber 2x3", 6), results.Inserts);
            Assert.Contains(new StockItem("Lumber 2x6", 3), results.Inserts);

            Assert.Contains(new StockItem("Lumber 2x2", 3), results.Updates);
            Assert.Contains(new StockItem("Lumber 2x4", 3), results.Updates);

            Assert.Contains(new StockItem("Lumber 2x10", 2), results.Deletes);
        }

        [Fact]
        public void ExcludePropertyNames()
        {
            const string EXCLUDED_PROPERTY_NAME = "Description";

            var hasher = new Sha1Hasher();
            var previousStock = _previousStock.Select(x => new StockItemWithDescription(x.Sku, x.Quantity, x.Sku)).Hash(hasher, new [] { EXCLUDED_PROPERTY_NAME });
            var actualStock = _actualStock.Select(x => new StockItemWithDescription(x.Sku, x.Quantity, x.Sku)).Hash(hasher, new [] { EXCLUDED_PROPERTY_NAME });
            var results = new Dispenser<StockItemWithDescription, string>().Dispense(actualStock, previousStock, x => x.Sku);

            Assert.NotNull(results);
            Assert.True(results.HasChanges);
            Assert.Contains(new StockItemWithDescription("Lumber 2x3", 6), results.Inserts);
            Assert.Contains(new StockItemWithDescription("Lumber 2x6", 3), results.Inserts);

            Assert.Contains(new StockItemWithDescription("Lumber 2x2", 3), results.Updates);
            Assert.Contains(new StockItemWithDescription("Lumber 2x4", 3), results.Updates);

            Assert.Contains(new StockItemWithDescription("Lumber 2x10", 2), results.Deletes);
        }
    }
}
