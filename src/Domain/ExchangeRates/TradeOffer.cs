namespace CurrencyExchangeRatesMonitor.Domain.ExchangeRates
{
    public class TradeOffer : Entity
    {
        public TradeOffer(Traider traider, Price price)
        {
            Contract.Requires(() => traider != null, $"{nameof(traider)} may not be null");
            Contract.Requires(() => price != null, $"{nameof(price)} may not be null");

            Traider = traider;
            Price = price;
        }

        public Traider Traider { get; }
        public Price Price { get; }

        public override string ToString() =>
            $"{{ {nameof(Id)}: {Id}, {nameof(Traider)}: {Traider}, {nameof(Price)}: {Price} }}";
    }
}