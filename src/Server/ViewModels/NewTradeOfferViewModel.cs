using System.Windows.Input;
using CurrencyExchangeRatesMonitor.Common.ViewModels;
using CurrencyExchangeRatesMonitor.Domain.ExchangeRates;
using CurrencyExchangeRatesMonitor.Server.Services;
using log4net;

namespace CurrencyExchangeRatesMonitor.Server.ViewModels
{
    public class NewTradeOfferViewModel : ViewModelBase
    {
        private readonly ILog log;
        private readonly TradeOffersService tradeOffersService;

        private string trader;
        private string baseCurrency;
        private string counterCurrency;
        private decimal bidPrice;
        private decimal askPrice;

        public NewTradeOfferViewModel(ILog log, TradeOffersService tradeOffersService)
        {
            this.log = log;
            this.tradeOffersService = tradeOffersService;

            AddNewTradeOfferCommand = new DelegateCommand(AddNewTradeOffer);
        }

        public string Traider
        {
            get { return trader; }
            set
            {
                trader = value;
                base.OnPropertyChanged(nameof(Traider));
            }
        }

        public string BaseCurrency
        {
            get { return baseCurrency; }
            set
            {
                baseCurrency = value;
                base.OnPropertyChanged(nameof(BaseCurrency));
            }
        }

        public string CounterCurrency
        {
            get { return counterCurrency; }
            set
            {
                counterCurrency = value;
                base.OnPropertyChanged(nameof(CounterCurrency));
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

        public ICommand AddNewTradeOfferCommand { get; }

        private void AddNewTradeOffer()
        {
            var newTradeOffer = new TradeOffer
            (
                new Traider(trader),
                new Price
                (
                    new CurrencyPair
                    (
                        new CurrencyUnit(baseCurrency),
                        new CurrencyUnit(counterCurrency)
                    ),
                    new ExecutablePrice(bidPrice),
                    new ExecutablePrice(askPrice)
                )
            );

            tradeOffersService.AddNewTradeOffer(newTradeOffer);

            ResetData();
        }

        private void ResetData()
        {
            Traider = null;
            BaseCurrency = null;
            CounterCurrency = null;
            BidPrice = 0;
            AskPrice = 0;
        }
    }
}