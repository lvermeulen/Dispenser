using System.Collections.Generic;

namespace Dispenser.Tests
{
    public class TypedValue<T>
    {
        public T Value { get; }

        public TypedValue(T t)
        {
            Value = t;
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
            var other = (TypedValue<T>)obj;
            return Equals(Value, other.Value);
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);
    }
}
