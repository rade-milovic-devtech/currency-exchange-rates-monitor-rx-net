using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using CurrencyExchangeRatesMonitor.Client.Hubs.ExchangeRates;
using CurrencyExchangeRatesMonitor.Common.Models;

namespace CurrencyExchangeRatesMonitor.Client.Repositories
{
    public class TradeOffersRepository
    {
        private readonly TradeOffersHubClient tradeOffersHubClient;

        public TradeOffersRepository(TradeOffersHubClient tradeOffersHubClient)
        {
            this.tradeOffersHubClient = tradeOffersHubClient;
        }

        public IObservable<IEnumerable<TradeOfferDto>> GetTradeOffersStream() =>
            Observable.Defer(() => tradeOffersHubClient.GetTradeOffersStream())
                .Catch(Observable.Return(new List<TradeOfferDto>()))
                .Repeat()
                .Publish()
                .RefCount();
    }
}