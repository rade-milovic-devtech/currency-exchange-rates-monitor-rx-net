namespace CurrencyExchangeRatesMonitor.Domain.ExchangeRates
{
    public class ExecutablePrice : ValueObject<ExecutablePrice>
    {
        public ExecutablePrice(decimal rate)
        {
            Contract.Requires(() => rate > 0.0m, $"{nameof(rate)} must be higher than 0");

            Rate = rate;
        }

        public decimal Rate { get; }

        public override bool EqualsCore(ExecutablePrice other) => Rate == other.Rate;

        public override int GetHashCodeCore() => Rate.GetHashCode();

        public override string ToString() => Rate.ToString();

        public static implicit operator decimal(ExecutablePrice currencyValue) => currencyValue.Rate;

        public static explicit operator ExecutablePrice(decimal rate) => new ExecutablePrice(rate);
    }
}