using System.Threading.Tasks;
using CurrencyExchangeRatesMonitor.Common.Constants;
using CurrencyExchangeRatesMonitor.Server.Services;
using log4net;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace CurrencyExchangeRatesMonitor.Server.Hubs.ExchangeRates
{
    [HubName(ServerConstants.TradeOffersHub)]
    public class TradeOffersHub : Hub
    {
        private readonly ILog log;
        private readonly TradeOffersService service;
        private readonly ContextHolder contextHolder;

        public const string TradeOffersGroupName = "AllTadeOfferClients";

        public TradeOffersHub(ILog log, TradeOffersService service, ContextHolder contextHolder)
        {
            this.log = log;
            this.service = service;
            this.contextHolder = contextHolder;
        }

        [HubMethodName(ServerConstants.TradeOffersSubscribe)]
        public async Task Subscribe()
        {
            contextHolder.TradeOfferClients = Clients;

            log.Info($"Received subscription from {Context.ConnectionId}.");

            await Groups.Add(Context.ConnectionId, TradeOffersGroupName);
            log.Info($"Connection {Context.ConnectionId} added to group '{TradeOffersGroupName}'.");

            var tradeOffers = service.GetLatestOffers();

            await Clients.Caller.SendTradeOffers(tradeOffers);

            log.Info($"Snapshot published to {Context.ConnectionId}.");
        }

        [HubMethodName(ServerConstants.TradeOffersUnsubscribe)]
        public async Task Unsubscibe()
        {
            log.Info($"Received unsubscription request from {Context.ConnectionId}.");

            await Groups.Remove(Context.ConnectionId, TradeOffersGroupName);

            log.Info($"Connection {Context.ConnectionId} has been removed.");
        }
    }
}