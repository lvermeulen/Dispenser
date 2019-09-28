using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Dispenser.Tests;

namespace DispenserChannelsTest
{
    public class Producer
    {
        private readonly ChannelWriter<StockItem> _writer;
        private readonly string _identifier;
        private readonly IEnumerable<StockItem> _stockItems;

        public Producer(ChannelWriter<StockItem> writer, string identifier, IEnumerable<StockItem> stockItems)
        {
            _writer = writer;
            _identifier = identifier;
            _stockItems = stockItems;
        }

        public async Task BeginProducingAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"PRODUCER ({_identifier}): Starting");

            foreach (var item in _stockItems)
            {
                Console.WriteLine($"PRODUCER ({_identifier}): Producing stock item with SKU {item.Sku}, quantity {item.Quantity}");
                await _writer.WriteAsync(item, cancellationToken);
            }

            Console.WriteLine($"PRODUCER ({_identifier}): Completed");
        }
    }
}