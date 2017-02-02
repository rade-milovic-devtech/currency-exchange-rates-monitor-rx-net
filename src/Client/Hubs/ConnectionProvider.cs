using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using log4net;

namespace CurrencyExchangeRatesMonitor.Client.Hubs
{
    public class ConnectionProvider : IDisposable
    {
        private readonly ILog log;
        private readonly string serverAddress;

        private readonly IObservable<Connection> connectionSequence;

        private readonly SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();

        public ConnectionProvider(ILog log, string serverAddress)
        {
            this.log = log;
            this.serverAddress = serverAddress;

            connectionSequence = CreateConnectionSequence();
        }

        public IObservable<Connection> ActiveConnection => connectionSequence;

        public void Dispose() => disposable.Dispose();

        private IObservable<Connection> CreateConnectionSequence()
        {
            var connectionSequence = Observable.Create<Connection>(observer =>
            {
               log.Info("Creating new connection...");

               var connection = GetNextConnection();
               var statusSubscription = connection.StatusStream.Subscribe(
                   _ => {},
                   _ => observer.OnCompleted(),
                   () =>
                   {
                       log.Info("Status subscription completed.");

                       observer.OnCompleted();
                   });

               var connectionSubscription = connection.Initialize().Subscribe(
                   _ => observer.OnNext(connection),
                   _ => observer.OnCompleted(),
                   observer.OnCompleted);

               return new CompositeDisposable { statusSubscription, connectionSubscription };
            })
            .Repeat()
            .Replay(1);

            var connected = 0;
            return Observable.Create<Connection>(observer =>
            {
                var subscription = connectionSequence.Subscribe(observer);

                if (Interlocked.CompareExchange(ref connected, 1, 0) == 0)
                {
                    if (!disposable.IsDisposed)
                    {
                        disposable.Disposable = connectionSequence.Connect();
                    }
                }

                return subscription;
            }).AsObservable();
        }

        private Connection GetNextConnection() => new Connection(serverAddress);
    }
}