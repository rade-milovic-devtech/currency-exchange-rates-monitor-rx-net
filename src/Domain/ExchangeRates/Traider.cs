using System;

namespace CurrencyExchangeRatesMonitor.Domain.ExchangeRates
{
    public class Traider : ValueObject<Traider>
    {
        public Traider(string name)
        {
            Contract.Requires(() => !string.IsNullOrWhiteSpace(name), $"{nameof(name)} may not be blank");

            Name = name;
        }

        public string Name { get; }

        public override bool EqualsCore(Traider other) => string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCodeCore() => Name.GetHashCode();

        public override string ToString() => Name;

        public static implicit operator string(Traider traider) => traider.Name;

        public static explicit operator Traider(string name) => new Traider(name);
    }
}