namespace CurrencyExchangeValuesMonitor.Domain.ExchangeRates
{
    public class CurrencyPair : ValueObject<CurrencyPair>
    {
        public CurrencyPair(CurrencyUnit @base, CurrencyUnit counter)
        {
            Contract.Requires(() => @base != null, $"{nameof(@base)} may not be null");
            Contract.Requires(() => counter != null, $"{nameof(counter)} may not be null");

            Symbol = $"{@base.Symbol.ToUpper()}/{counter.Symbol.ToUpper()}";
            Base = @base;
            Counter = counter;
        }

        public string Symbol { get; }
        public CurrencyUnit Base { get; }
        public CurrencyUnit Counter { get; }

        public override bool EqualsCore(CurrencyPair other) =>
            Symbol == other.Symbol &&
            Base == other.Base &&
            Counter == other.Counter;

        public override int GetHashCodeCore()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ (string.IsNullOrEmpty(Symbol) ? 0 : Symbol.GetHashCode());
                hashCode = (hashCode * 397) ^ Base.GetHashCode();
                hashCode = (hashCode * 397) ^ Counter.GetHashCode();

                return hashCode;
            }
        }

        public override string ToString() =>
            $"{{ {nameof(Symbol)}: {Symbol}, {nameof(Base)}: {Base}, {nameof(Counter)}: {Counter} }}";
    }
}