using Autofac;
using CurrencyExchangeRatesMonitor.Client.Hubs;
using CurrencyExchangeRatesMonitor.Client.Hubs.ExchangeRates;
using CurrencyExchangeRatesMonitor.Client.Repositories;
using CurrencyExchangeRatesMonitor.Client.ViewModels;
using CurrencyExchangeRatesMonitor.Common.Constants;

namespace CurrencyExchangeRatesMonitor.Client.Config
{
    public class IocBootstrapper
    {
        public IContainer Build()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<LoggingModule>();

            containerBuilder.RegisterType<ConnectionProvider>()
                .WithParameter("serverAddress", ClientConstants.ServerAddress)
                .SingleInstance();
            containerBuilder.RegisterType<TradeOffersHubClient>().SingleInstance();

            containerBuilder.RegisterType<TradeOffersRepository>().SingleInstance();

            containerBuilder.RegisterType<ConnectivityStatusViewModel>().SingleInstance();
            containerBuilder.RegisterType<TradeOffersViewModel>().SingleInstance();
            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();

            containerBuilder.RegisterType<MainWindow>().SingleInstance();

            return containerBuilder.Build();
        }
    }
}