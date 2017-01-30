using System.Threading;
using System.Threading.Tasks;
using CurrencyExchangeRatesMonitor.Server.Services;
using log4net;

namespace CurrencyExchangeRatesMonitor.Server.Hubs.ExchangeRates
{
    public class TradeOffersPublisher
    {
        private const int tradeOffersPublishingDelay = 500;

        private readonly TradeOffersService service;
        private readonly ContextHolder contextHolder;
        private ILog log;

        private CancellationTokenSource autoRunningCancellationToken;
        private Task autoRunningTask;

        public TradeOffersPublisher(TradeOffersService service, ContextHolder contextHolder, ILog log)
        {
            this.service = service;
            this.contextHolder = contextHolder;
            this.log = log;
        }

        public void Start()
        {
            autoRunningCancellationToken = new CancellationTokenSource();
            autoRunningTask = Task.Run(async () =>
                {
                    while (!autoRunningCancellationToken.IsCancellationRequested)
                    {
                        await SendLatestTradeOffers();
                        await Task.Delay(tradeOffersPublishingDelay, autoRunningCancellationToken.Token);
                    }
                }, autoRunningCancellationToken.Token);
        }

        public void Stop()
        {
            if (autoRunningCancellationToken != null)
            {
                autoRunningCancellationToken.Cancel();
                autoRunningCancellationToken.Dispose();
                autoRunningCancellationToken = null;
            }
        }

        private Task SendLatestTradeOffers()
        {
            if (contextHolder.TradeOfferClients == null) return Task.FromResult(false);

            var tradeOffers = service.GetLatestOffers();

            log.Info("Broadcasting new trade offers to clients.");

            return contextHolder.TradeOfferClients.Group(TradeOffersHub.TradeOffersGroupName).SendTradeOffers(tradeOffers);
        }
    }
}