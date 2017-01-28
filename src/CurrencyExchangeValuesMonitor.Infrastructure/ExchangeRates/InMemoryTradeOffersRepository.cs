﻿using System.Collections.Generic;
using CurrencyExchangeValuesMonitor.Domain;
using CurrencyExchangeValuesMonitor.Domain.ExchangeRates;

namespace CurrencyExchangeValuesMonitor.Infrastructure.ExchangeRates
{
    public class InMemoryTradeOffersRepository : TradeOffersRepository
    {
        private readonly ICollection<TradeOffer> tradeOffers = new HashSet<TradeOffer>();

        public IEnumerable<TradeOffer> All => tradeOffers;

        public void Add(TradeOffer tradeOffer)
        {
            Contract.Requires(() => tradeOffer != null, $"{nameof(tradeOffer)} may not be null");

            tradeOffers.Add(tradeOffer);
        }
    }
}