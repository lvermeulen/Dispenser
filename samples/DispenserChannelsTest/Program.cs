using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Dispenser;
using Dispenser.Hasher.Sha256;
using Dispenser.Tests;

namespace DispenserChannelsTest
{
    public static class Program
    {
        private static readonly StockItem[] s_previousStock =
        {
            new StockItem("Lumber 2x2", 7),
            new StockItem("Lumber 2x4", 5),
            new StockItem("Lumber 2x8", 4),
            new StockItem("Lumber 2x10", 2)
        };

        private static readonly StockItem[] s_actualStock =
        {
            new StockItem("Lumber 2x2", 3),
            new StockItem("Lumber 2x3", 6),
            new StockItem("Lumber 2x4", 3),
            new StockItem("Lumber 2x6", 3),
            new StockItem("Lumber 2x8", 4)
        };

        public static async Task Main()
        {
            const int capacity = 10;
            const BoundedChannelFullMode fullModeOption = BoundedChannelFullMode.Wait;
            const bool isSingleReader = false;
            const bool isSingleWriter = false;
            const bool allowSynchronousContinuations = true;

            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = fullModeOption,
                SingleReader = isSingleReader,
                SingleWriter = isSingleWriter,
                AllowSynchronousContinuations = allowSynchronousContinuations
            };

            var channel = Channel.CreateBounded<StockItem>(options);

            var hasher = new Sha256Hasher();
            var results = new Dispenser<StockItem, string>().Dispense(s_actualStock.Hash(hasher), s_previousStock.Hash(hasher), x => x.Sku);

            Console.WriteLine("Insert results:");
            foreach (var result in results.Inserts)
            {
                Console.WriteLine($"\tStock item with SKU {result.Sku}, quantity {result.Quantity}");
            }
            Console.WriteLine("Update results:");
            foreach (var result in results.Updates)
            {
                Console.WriteLine($"\tStock item with SKU {result.Sku}, quantity {result.Quantity}");
            }
            Console.WriteLine("Delete results:");
            foreach (var result in results.Deletes)
            {
                Console.WriteLine($"\tStock item with SKU {result.Sku}, quantity {result.Quantity}");
            }
            Console.WriteLine();

            var insertProducer = new Producer(channel.Writer, "inserts", results.Inserts);
            var updateProducer = new Producer(channel.Writer, "updates", results.Updates);
            var deleteProducer = new Producer(channel.Writer, "deletes", results.Deletes);

            var insertConsumer = new Consumer(channel.Reader, "inserts");
            var updateConsumer = new Consumer(channel.Reader, "updates");
            var deleteConsumer = new Consumer(channel.Reader, "deletes");

            var insertConsumerTask = insertConsumer.ConsumeDataAsync();
            var updateConsumerTask = updateConsumer.ConsumeDataAsync();
            var deleteConsumerTask = deleteConsumer.ConsumeDataAsync();

            using (var tokenSource = new CancellationTokenSource())
            {
                var insertProducerTask = insertProducer.BeginProducingAsync(tokenSource.Token);
                var updateProducerTask = updateProducer.BeginProducingAsync(tokenSource.Token);
                var deleteProducerTask = deleteProducer.BeginProducingAsync(tokenSource.Token);

                await Task
                    .WhenAll(insertProducerTask, updateProducerTask, deleteProducerTask)
                    .ContinueWith(_ => channel.Writer.Complete(), tokenSource.Token)
                    .ConfigureAwait(false);
            }

            await Task.WhenAll(insertConsumerTask, updateConsumerTask, deleteConsumerTask).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
