using System;
using System.Threading.Channels;
using System.Threading.Tasks;
using Dispenser.Tests;

namespace DispenserChannelsTest
{
    public class Consumer
    {
        private readonly ChannelReader<StockItem> _reader;
        private readonly string _identifier;

        public Consumer(ChannelReader<StockItem> reader, string identifier)
        {
            _reader = reader;
            _identifier = identifier;
        }

        public async Task ConsumeDataAsync()
        {
            Console.WriteLine($"CONSUMER ({_identifier}): Starting");

            while (await _reader.WaitToReadAsync())
            {
                if (_reader.TryRead(out var stockItem))
                {
                    Console.WriteLine($"CONSUMER ({_identifier}): Processing stock item with SKU {stockItem.Sku}, quantity {stockItem.Quantity}");
                }
            }

            Console.WriteLine($"CONSUMER ({_identifier}): Completed");
        }
    }
}
