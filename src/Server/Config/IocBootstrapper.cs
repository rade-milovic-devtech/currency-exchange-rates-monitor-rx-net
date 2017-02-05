using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using CurrencyExchangeRatesMonitor.Common.Ioc;
using CurrencyExchangeRatesMonitor.Domain.ExchangeRates;
using CurrencyExchangeRatesMonitor.Infrastructure.ExchangeRates;
using CurrencyExchangeRatesMonitor.Server.Hubs;
using CurrencyExchangeRatesMonitor.Server.Hubs.ExchangeRates;
using CurrencyExchangeRatesMonitor.Server.Services;
using CurrencyExchangeRatesMonitor.Server.ViewModels;

namespace CurrencyExchangeRatesMonitor.Server.Config
{
    public class IocBootstrapper
    {
        public IContainer Build()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<LoggingModule>();

            containerBuilder.RegisterType<InMemoryTradeOffersRepository>().As<TradeOffersRepository>().SingleInstance();

            containerBuilder.RegisterType<TradeOffersService>().SingleInstance();

            containerBuilder.RegisterType<ContextHolder>().SingleInstance();
            containerBuilder.RegisterType<TradeOffersPublisher>().SingleInstance();
            containerBuilder.RegisterHubs(Assembly.GetExecutingAssembly());

            containerBuilder.RegisterType<NewTradeOfferViewModel>().SingleInstance();
            containerBuilder.RegisterType<MainWindowViewModel>().SingleInstance();
            containerBuilder.RegisterType<MainWindow>().SingleInstance();

            return containerBuilder.Build();
        }
    }
}