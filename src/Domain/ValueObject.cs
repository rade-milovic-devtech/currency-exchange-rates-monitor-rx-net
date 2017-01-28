namespace CurrencyExchangeRatesMonitor.Domain
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;

            if (ReferenceEquals(valueObject, null)) return false;

            return EqualsCore(valueObject);
        }

        public override int GetHashCode() => GetHashCodeCore();

        public abstract bool EqualsCore(T other);

        public abstract int GetHashCodeCore();

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject<T> left, ValueObject<T> right) => !(left == right);
    }
}