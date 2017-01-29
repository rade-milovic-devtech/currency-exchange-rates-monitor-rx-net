using System;

namespace CurrencyExchangeRatesMonitor.Common.Models
{
    public class TradeOfferDto
    {
        public Guid Id { get; set; }
        public string TraiderName { get; set; }
        public string CurrencyPair { get; set; }
        public decimal BidPrice { get; set; }
        public decimal AskPrice { get; set; }
        public decimal MidPrice { get; set; }
        public DateTimeOffset PriceDate { get; set; }
    }
}