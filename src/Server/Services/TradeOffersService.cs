using System.Collections.Generic;
using System.Linq;
using CurrencyExchangeRatesMonitor.Common.Models;
using CurrencyExchangeRatesMonitor.Domain.ExchangeRates;

namespace CurrencyExchangeRatesMonitor.Server.Services
{
    public class TradeOffersService
    {
        private readonly TradeOffersRepository repository;

        public TradeOffersService(TradeOffersRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<TradeOfferDto> GetLatestOffers() =>
            repository.All
                .GroupBy(tradeOffer => new { tradeOffer.Traider, tradeOffer.Price.Pair })
                .Select(group =>
                    group.OrderByDescending(tradeOffer => tradeOffer.Price.ValueDate)
                         .First()
                ).Select(tradeOffer =>
                    new TradeOfferDto
                    {
                        Id = tradeOffer.Id,
                        TraiderName = tradeOffer.Traider,
                        CurrencyPair = tradeOffer.Price.Pair.Symbol,
                        BidPrice = tradeOffer.Price.Bid,
                        AskPrice = tradeOffer.Price.Ask,
                        MidPrice = tradeOffer.Price.Mid,
                        PriceDate = tradeOffer.Price.ValueDate
                    });

        public void AddNewTradeOffer(TradeOffer tradeOffer) => repository.Add(tradeOffer);
    }
}