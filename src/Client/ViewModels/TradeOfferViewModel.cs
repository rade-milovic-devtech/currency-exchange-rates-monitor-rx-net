using CurrencyExchangeRatesMonitor.Common.ViewModels;

namespace CurrencyExchangeRatesMonitor.Client.ViewModels
{
    public class TradeOfferViewModel : ViewModelBase
    {
        private string traiderName;
        public string currencyPair;
        public decimal bidPrice;
        public decimal askPrice;
        public decimal midPrice;

        public string TraiderName
        {
            get { return traiderName; }
            set
            {
                traiderName = value;
                base.OnPropertyChanged(nameof(TraiderName));
            }
        }

        public string CurrencyPair
        {
            get { return currencyPair; }
            set
            {
                currencyPair = value;
                base.OnPropertyChanged(nameof(CurrencyPair));
            }
        }

        public decimal BidPrice
        {
            get { return bidPrice; }
            set
            {
                bidPrice = value;
                base.OnPropertyChanged(nameof(BidPrice));
            }
        }

        public decimal AskPrice
        {
            get { return askPrice; }
            set
            {
                askPrice = value;
                base.OnPropertyChanged(nameof(AskPrice));
            }
        }

        public decimal MidPrice
        {
            get { return midPrice; }
            set
            {
                midPrice = value;
                base.OnPropertyChanged(nameof(MidPrice));
            }
        }
    }
}