namespace CurrencyExchangeRatesMonitor.Client.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(ConnectivityStatusViewModel connectivityStatusViewModel,
            TradeOffersViewModel tradeOffersViewModel)
        {
            ConnectivityStatusViewModel = connectivityStatusViewModel;
            TradeOffersViewModel = tradeOffersViewModel;
        }

        public ConnectivityStatusViewModel ConnectivityStatusViewModel { get; }
        public TradeOffersViewModel TradeOffersViewModel { get; }
    }
}