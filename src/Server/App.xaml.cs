using System.Threading;
using System.Windows;
using Autofac;
using CurrencyExchangeRatesMonitor.Server.Config;
using CurrencyExchangeRatesMonitor.Server.Hubs;
using CurrencyExchangeRatesMonitor.Server.ViewModels;
using log4net;
using log4net.Config;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace CurrencyExchangeRatesMonitor.Server
{
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        public static IContainer Container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container = new IocBootstrapper().Build();

            InitializeLogging();

            Start();

            log.Info("Server application started.");
        }

        private void Start()
        {
            var vm = Container.Resolve<MainWindowViewModel>();

            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.DataContext = vm;

            mainWindow.Show();
        }

        private void InitializeLogging()
        {
            Thread.CurrentThread.Name = "UI";

            XmlConfigurator.Configure();
        }
    }
}