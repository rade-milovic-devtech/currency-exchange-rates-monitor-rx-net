using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CurrencyExchangeRatesMonitor.Common.Constants;
using log4net;
using Microsoft.AspNet.SignalR.Client;

namespace CurrencyExchangeRatesMonitor.Client.Hubs
{
    public class Connection
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Connection));

        private readonly ISubject<ConnectionInfo> statusStream;
        private readonly HubConnection hubConnection;

        private bool initialized;

        public Connection(string serverAddress)
        {
            hubConnection = new HubConnection(serverAddress);
            hubConnection.Error += exception => log.Error($"There was a connection error with {serverAddress}", exception);

            statusStream = new BehaviorSubject<ConnectionInfo>(new ConnectionInfo(serverAddress, ConnectionStatus.Uninitialized));
            CreateConnectionStatusesSequence().Subscribe(
                connectionSequence => statusStream.OnNext(new ConnectionInfo(serverAddress, connectionSequence)),
                statusStream.OnError,
                statusStream.OnCompleted);

            TradeOffersHubProxy = hubConnection.CreateHubProxy(ServerConstants.TradeOffersHub);

            ServerAddress = serverAddress;
        }

        public string ServerAddress { get; }

        public IHubProxy TradeOffersHubProxy { get; }

        public IObservable<ConnectionInfo> StatusStream => statusStream;

        public IObservable<Unit> Initialize()
        {
            if (initialized)
            {
                throw new InvalidOperationException("Connection has already been initialized.");
            }

            initialized = true;

            return Observable.Create<Unit>(async observer =>
            {
                statusStream.OnNext(new ConnectionInfo(ServerAddress, ConnectionStatus.Connecting));

                try
                {
                    log.Info($"Connecting to {ServerAddress}");

                    await hubConnection.Start();

                    statusStream.OnNext(new ConnectionInfo(ServerAddress, ConnectionStatus.Connected));

                    observer.OnNext(Unit.Default);
                }
                catch (Exception ex)
                {
                    log.Error("An error occurred when starting SignalR connection", ex);

                    observer.OnError(ex);
                }

                return Disposable.Create(() =>
                {
                    try
                    {
                        log.Info("Stoping connection...");
                        hubConnection.Stop();
                        log.Info("Connection stopped");
                    }
                    catch (Exception ex)
                    {
                        log.Error("An error occurred while stoping connection", ex);
                    }
                });
            })
            .Publish()
            .RefCount();
        }

        public override string ToString() => $"{nameof(ServerAddress)}: {ServerAddress}";

        private IObservable<ConnectionStatus> CreateConnectionStatusesSequence()
        {
            var disconnectedConnectionSequence = Observable.FromEvent(
                addHandler => hubConnection.Closed += addHandler,
                removeHandler => hubConnection.Closed -= removeHandler
            ).Select(_ => ConnectionStatus.Disconnected);

            var reconnectedConnectionSequence = Observable.FromEvent(
                addHandler => hubConnection.Reconnected += addHandler,
                removeHandler => hubConnection.Closed -= removeHandler
            ).Select(_ => ConnectionStatus.Reconnected);

            var reconnectingConnectionSequence = Observable.FromEvent(
                addHandler => hubConnection.Closed += addHandler,
                removeHandler => hubConnection.Reconnecting -= removeHandler
            ).Select(_ => ConnectionStatus.Reconnecting);

            var errorConnectionSequence = Observable.FromEvent<Exception>(
                addHandler => hubConnection.Error += addHandler,
                removeHandler => hubConnection.Error -= removeHandler
            ).Select(_ => ConnectionStatus.Disconnected);

            var allConnectionSequences = Observable.Merge(
                disconnectedConnectionSequence,
                reconnectedConnectionSequence,
                reconnectingConnectionSequence,
                errorConnectionSequence
            );

            return Observable.Create<ConnectionStatus>(
                observer => allConnectionSequences.Subscribe(
                    connectionStatus =>
                    {
                        observer.OnNext(connectionStatus);

                        if (connectionStatus == ConnectionStatus.Disconnected)
                            observer.OnCompleted();
                    },
                    observer.OnError,
                    observer.OnCompleted
                )
            );
        }
    }
}