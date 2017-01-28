using System.Collections.Generic;

namespace CurrencyExchangeValuesMonitor.Domain.ExchangeRates
{
    public interface TradeOffersRepository
    {
        IEnumerable<TradeOffer> All { get; }
        void Add(TradeOffer tradeOffer);
    }
}