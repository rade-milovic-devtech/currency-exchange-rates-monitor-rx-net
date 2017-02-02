using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CurrencyExchangeRatesMonitor.Client.Hubs;
using CurrencyExchangeRatesMonitor.Common.ViewModels;
using log4net;

namespace CurrencyExchangeRatesMonitor.Client.ViewModels
{
    public class ConnectivityStatusViewModel : ViewModelBase
    {
        private readonly ILog log;
        private readonly ConnectionProvider connectionProvider;

        private string status = "Connecting...";
        private bool disconnected = true;

        public ConnectivityStatusViewModel(ILog log, ConnectionProvider connectionProvider)
        {
            this.log = log;
            this.connectionProvider = connectionProvider;

            connectionProvider.ActiveConnection
                .Do(_ => log.Info("New connection created by connection provider"))
                .Select(connection => connection.StatusStream)
                .Switch()
                .Publish()
                .RefCount()
                .ObserveOn(DispatcherScheduler.Current)
                .SubscribeOn(TaskPoolScheduler.Default)
                .Subscribe(
                    OnStatusChange,
                    exception => log.Error("An error occurred within the connection status stream.", exception)
                );
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                base.OnPropertyChanged(nameof(Status));
            }
        }

        public bool Disconnected
        {
            get { return disconnected; }
            set
            {
                disconnected = value;
                base.OnPropertyChanged(nameof(Disconnected));
            }
        }

        private void OnStatusChange(ConnectionInfo connectionInfo)
        {
            switch (connectionInfo.ConnectionStatus)
            {
                case ConnectionStatus.Uninitialized:
                case ConnectionStatus.Connecting:
                    Status = "Connecting...";
                    Disconnected = true;
                    break;
                case ConnectionStatus.Reconnected:
                case ConnectionStatus.Connected:
                    Status = "Connected";
                    Disconnected = false;
                    break;
                case ConnectionStatus.Reconnecting:
                    Status = "Reconnecting...";
                    Disconnected = true;
                    break;
                case ConnectionStatus.Disconnected:
                    Status = "Disconnected";
                    Disconnected = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}