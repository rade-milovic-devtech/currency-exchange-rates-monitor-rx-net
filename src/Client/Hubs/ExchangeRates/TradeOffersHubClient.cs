using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CurrencyExchangeRatesMonitor.Common.Constants;
using CurrencyExchangeRatesMonitor.Common.Models;
using log4net;
using Microsoft.AspNet.SignalR.Client;

namespace CurrencyExchangeRatesMonitor.Client.Hubs.ExchangeRates
{
    public class TradeOffersHubClient
    {
        private readonly ILog log;
        private readonly ConnectionProvider connectionProvider;

        public TradeOffersHubClient(ILog log, ConnectionProvider connectionProvider)
        {
            this.log = log;
            this.connectionProvider = connectionProvider;
        }

        public IObservable<IEnumerable<TradeOfferDto>> GetTradeOffersStream() =>
            connectionProvider.ActiveConnection
                .SelectMany(connection => GetTradeOffers(connection.TradeOffersHubProxy));

        private IObservable<IEnumerable<TradeOfferDto>> GetTradeOffers(IHubProxy tradeOffersHubProxy)
        {
            return Observable.Create<IEnumerable<TradeOfferDto>>(observer =>
            {
                var tradeOffersSendSubscription = tradeOffersHubProxy.On<IEnumerable<TradeOfferDto>>(
                   "SendTradeOffers", observer.OnNext);

                log.Info("Sending trade offer subscription...");
                var sendSubscriptionDisposable = SendSubscription(tradeOffersHubProxy)
                    .Subscribe(
                        _ => log.Info("Subscribed to trade offers."),
                        observer.OnError);

                var unsubscriptionDisposable = Disposable.Create(() =>
                {
                    log.Info("Sending trade offers unsubscription...");
                    SendUnsubscription(tradeOffersHubProxy)
                        .Subscribe(
                            _ => log.Info("Unsubscribed from trade offers."),
                            exception => log.Warn($"An error occurred while unsubscribing from ticker: {exception.Message}")
                        );
                });

                return new CompositeDisposable {
                    tradeOffersSendSubscription,
                    sendSubscriptionDisposable,
                    unsubscriptionDisposable
                };
            })
            .Publish()
            .RefCount();
        }

        private static IObservable<Unit> SendSubscription(IHubProxy tradeOffersHubProxy) =>
            Observable.FromAsync(() => tradeOffersHubProxy.Invoke(ServerConstants.TradeOffersSubscribe));

        private static IObservable<Unit> SendUnsubscription(IHubProxy tradeOffersHubProxy) =>
            Observable.FromAsync(() => tradeOffersHubProxy.Invoke(ServerConstants.TradeOffersUnsubscribe));
    }
}