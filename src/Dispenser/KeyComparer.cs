using System;
using System.Collections.Generic;

namespace Dispenser
{
    public class KeyComparer<T, TKeyProperty> : IEqualityComparer<T>
    {
        private readonly Func<T, TKeyProperty> _keyPropertySelector;

        public KeyComparer(Func<T, TKeyProperty> keyPropertySelector)
        {
            _keyPropertySelector = keyPropertySelector;
        }

        public bool Equals(T x, T y)
        {
            if (EqualityComparer<T>.Default.Equals(x, default) && EqualityComparer<T>.Default.Equals(y, default))
            {
                return true;
            }

            if (EqualityComparer<T>.Default.Equals(x, default) || EqualityComparer<T>.Default.Equals(y, default))
            {
                return false;
            }

            var xKeyValue = _keyPropertySelector(x);
            var yKeyValue = _keyPropertySelector(y);

            if (EqualityComparer<TKeyProperty>.Default.Equals(xKeyValue, default)
                && EqualityComparer<TKeyProperty>.Default.Equals(yKeyValue, default))
            {
                return true;
            }

            if (EqualityComparer<TKeyProperty>.Default.Equals(xKeyValue, default)
                || EqualityComparer<TKeyProperty>.Default.Equals(yKeyValue, default))
            {
                return false;
            }

            return xKeyValue.Equals(yKeyValue);
        }

        public int GetHashCode(T obj) => EqualityComparer<T>.Default.Equals(obj, default) || EqualityComparer<TKeyProperty>.Default.Equals(_keyPropertySelector(obj), default)
            ? base.GetHashCode()
            : _keyPropertySelector(obj).GetHashCode();
    }
}
