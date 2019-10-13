using System;

namespace Dispenser.Tests
{
    public class StockItem
    {
        public string Sku { get; }
        public int Quantity { get; }

        public StockItem(string sku)
        {
            Sku = sku;
        }

        public StockItem(string sku, int quantity)
            : this(sku)
        {
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            var other = (StockItem)obj;
            return string.Equals(Sku, other.Sku, StringComparison.OrdinalIgnoreCase) 
                && Quantity == other.Quantity;
        }

        public override int GetHashCode() => 
            Sku.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
    }
}
