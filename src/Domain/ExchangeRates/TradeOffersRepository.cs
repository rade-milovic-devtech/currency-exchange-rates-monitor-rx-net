using System.Collections.Generic;

namespace CurrencyExchangeRatesMonitor.Domain.ExchangeRates
{
    public interface TradeOffersRepository
    {
        IEnumerable<TradeOffer> All { get; }
        void Add(TradeOffer tradeOffer);
    }
}