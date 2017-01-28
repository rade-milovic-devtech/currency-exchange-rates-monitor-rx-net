using System;

namespace CurrencyExchangeRatesMonitor.Domain
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;

            if (ReferenceEquals(other, null)) return false;

            if (ReferenceEquals(this, other)) return true;

            if (GetType() != other.GetType()) return false;

            if (Id == Guid.Empty || other.Id == Guid.Empty) return false;

            return Id == other.Id;
        }

        public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();

        public static bool operator ==(Entity left, Entity right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null)) return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;

            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right) => !(left == right);
    }
}