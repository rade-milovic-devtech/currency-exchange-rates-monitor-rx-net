using System;

namespace CurrencyExchangeValuesMonitor.Domain.ExchangeRates
{
    public class Price : ValueObject<Price>
    {
        public Price(CurrencyPair pair, ExecutablePrice bid, ExecutablePrice ask)
        {
            Contract.Requires(() => pair != null, $"{nameof(pair)} may not be null");
            Contract.Requires(() => bid != null, $"{nameof(bid)} may not be null");
            Contract.Requires(() => ask != null, $"{nameof(ask)} may not be null");

            Pair = pair;
            Bid = bid;
            Ask = ask;
            ValueDate = DateTimeOffset.UtcNow;
        }

        public CurrencyPair Pair { get; }
        public ExecutablePrice Bid { get; }
        public ExecutablePrice Ask { get; }
        public ExecutablePrice Mid => new ExecutablePrice((Bid.Rate + Ask.Rate) / 2);
        public DateTimeOffset ValueDate { get; }

        public override bool EqualsCore(Price other) =>
            Pair == other.Pair &&
            Bid == other.Bid &&
            Ask == other.Ask &&
            ValueDate == other.ValueDate;

        public override int GetHashCodeCore()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ Pair.GetHashCode();
                hashCode = (hashCode * 397) ^ Bid.GetHashCode();
                hashCode = (hashCode * 397) ^ Ask.GetHashCode();
                hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();

                return hashCode;
            }
        }

        public override string ToString() =>
            $"{{ {nameof(Pair)}: {Pair}, {nameof(Bid)}: {Bid}, {nameof(Ask)}: {Ask}, {nameof(Mid)}: {Mid}, {nameof(ValueDate)}: {ValueDate} }}";
    }
}