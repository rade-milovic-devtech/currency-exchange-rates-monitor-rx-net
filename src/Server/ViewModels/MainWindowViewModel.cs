using System;
using System.Windows.Input;
using CurrencyExchangeRatesMonitor.Common.Constants;
using CurrencyExchangeRatesMonitor.Common.ViewModels;
using CurrencyExchangeRatesMonitor.Server.Hubs.ExchangeRates;
using log4net;
using Microsoft.Owin.Hosting;

namespace CurrencyExchangeRatesMonitor.Server.ViewModels
{
    public class MainWindowViewModel
    {
        private readonly ILog log;
        private readonly TradeOffersPublisher tradeOffersPublisher;

        private IDisposable signalrProcess;

        public MainWindowViewModel(ILog log, TradeOffersPublisher tradeOffersPublisher)
        {
            this.log = log;
            this.tradeOffersPublisher = tradeOffersPublisher;

            StartServerCommand = new DelegateCommand(StartServer);
            StopServerCommand = new DelegateCommand(StopServer);
        }

        public ICommand StartServerCommand { get; }
        public ICommand StopServerCommand { get; }

        private void StartServer()
        {
            try
            {
                signalrProcess = WebApp.Start(ServerConstants.Address);

                tradeOffersPublisher.Start();

                log.Info($"Server is started on address {ServerConstants.Address}.");
            }
            catch (Exception ex)
            {
                log.Error("An error occured while starting SignalR Server process.", ex);
            }
        }

        private void StopServer()
        {
            if (signalrProcess != null)
            {
                tradeOffersPublisher.Stop();
                signalrProcess.Dispose();
                signalrProcess = null;

                log.Info($"Server is stopped on address {ServerConstants.Address}.");
            }
        }
    }
}