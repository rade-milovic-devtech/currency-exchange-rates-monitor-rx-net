using System;

namespace CurrencyExchangeRatesMonitor.Domain.ExchangeRates
{
    public class CurrencyUnit : ValueObject<CurrencyUnit>
    {
        public CurrencyUnit(string symbol)
        {
            Contract.Requires(() => !string.IsNullOrWhiteSpace(symbol), $"{nameof(symbol)} may not be blank");

            Symbol = symbol;
        }

        public string Symbol { get; }

        public override bool EqualsCore(CurrencyUnit other) => string.Equals(Symbol, other.Symbol, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCodeCore() => Symbol.GetHashCode();

        public override string ToString() => Symbol;

        public static implicit operator string(CurrencyUnit currencyUnit) => currencyUnit.Symbol;

        public static explicit operator CurrencyUnit(string code) => new CurrencyUnit(code);
    }
}