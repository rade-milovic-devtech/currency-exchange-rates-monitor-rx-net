using System.Threading;
using System.Windows;
using Autofac;
using CurrencyExchangeRatesMonitor.Client.Config;
using log4net;
using log4net.Config;

namespace CurrencyExchangeRatesMonitor.Client
{
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeLogging();

            Start();

            log.Info("Server application started.");
        }

        private void Start()
        {
            var container = new IocBootstrapper().Build();

            var mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }

        private void InitializeLogging()
        {
            Thread.CurrentThread.Name = "UI";

            XmlConfigurator.Configure();
        }
    }
}