using Microsoft.AspNet.SignalR.Hubs;

namespace CurrencyExchangeRatesMonitor.Server.Hubs
{
    public class ContextHolder
    {
        public IHubCallerConnectionContext<dynamic> TradeOfferClients { get; set; }
    }
}