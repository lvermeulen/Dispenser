using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable PossibleMultipleEnumeration

namespace Dispenser
{
    public class Dispenser<T, TKeyProperty>
    {
        public class Results
        {
            public IEnumerable<T> Inserts { get; set; } = Enumerable.Empty<T>();
            public IEnumerable<T> Updates { get; set; } = Enumerable.Empty<T>();
            public IEnumerable<T> Deletes { get; set; } = Enumerable.Empty<T>();

            public bool HasChanges => Inserts.Any() || Updates.Any() || Deletes.Any();
        }

        public Results Dispense(IEnumerable<HashedPair<T>> sourceValues, IEnumerable<HashedPair<T>> targetValues, Func<T, TKeyProperty> keyPropertySelector)
        {
            if (sourceValues == null)
            {
                throw new ArgumentNullException(nameof(sourceValues));
            }
            if (targetValues == null)
            {
                throw new ArgumentNullException(nameof(targetValues));
            }
            if (keyPropertySelector == null)
            {
                throw new ArgumentNullException(nameof(keyPropertySelector));
            }

            var updates = from sourceResult in sourceValues
                join targetResult in targetValues on keyPropertySelector(sourceResult.Value) equals keyPropertySelector(targetResult.Value)
                where sourceResult.HashValue != targetResult.HashValue
                select sourceResult;

            var uniqueComparer = new KeyComparer<T, TKeyProperty>(keyPropertySelector);

            return new Results
            {
                Inserts = sourceValues.Select(x => x.Value).Except(targetValues.Select(x => x.Value), uniqueComparer),
                Updates = updates.Select(x => x.Value),
                Deletes = targetValues.Select(x => x.Value).Except(sourceValues.Select(x => x.Value), uniqueComparer)
            };
        }
    }
}
