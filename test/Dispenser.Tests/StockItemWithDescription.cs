namespace Dispenser.Tests
{
    public class StockItemWithDescription : StockItem
    {
        public string Description { get; }

        protected StockItemWithDescription(string sku) 
            : base(sku)
        { }

        public StockItemWithDescription(string sku, int quantity) 
            : base(sku, quantity)
        { }

        public StockItemWithDescription(string sku, int quantity, string description)
            : this(sku, quantity)
        {
            Description = description;
        }
    }
}
