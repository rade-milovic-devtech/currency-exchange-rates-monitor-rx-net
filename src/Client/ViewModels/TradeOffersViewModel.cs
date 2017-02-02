using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using CurrencyExchangeRatesMonitor.Client.Repositories;
using CurrencyExchangeRatesMonitor.Common.Models;
using CurrencyExchangeRatesMonitor.Common.ViewModels;
using log4net;

namespace CurrencyExchangeRatesMonitor.Client.ViewModels
{
    public class TradeOffersViewModel : ViewModelBase
    {
        private readonly ILog log;
        private readonly TradeOffersRepository tradeOffersRepository;

        private ObservableCollection<TradeOfferViewModel> tradeOffers = new ObservableCollection<TradeOfferViewModel>();

        public TradeOffersViewModel(ILog log, TradeOffersRepository tradeOffersRepository)
        {
            this.log = log;
            this.tradeOffersRepository = tradeOffersRepository;

            LoadTradeOffers();
        }

        public ObservableCollection<TradeOfferViewModel> TradeOffers
        {
            get { return tradeOffers; }
            set
            {
                tradeOffers = value;
                base.OnPropertyChanged(nameof(TradeOffers));
            }
        }

        private void LoadTradeOffers()
        {
            tradeOffersRepository.GetTradeOffersStream()
                .ObserveOn(DispatcherScheduler.Current)
                .SubscribeOn(TaskPoolScheduler.Default)
                .Subscribe(
                    LoadTradeOffers,
                    exception => log.Error("An error occurred within the trade stream", exception)
                );
        }

        private void LoadTradeOffers(IEnumerable<TradeOfferDto> tradeOffers)
        {
            var tradeOffersViewModel = tradeOffers.Select(tradeOffer =>
                new TradeOfferViewModel
                {
                    TraiderName = tradeOffer.TraiderName,
                    CurrencyPair = tradeOffer.CurrencyPair,
                    BidPrice = tradeOffer.BidPrice,
                    AskPrice = tradeOffer.AskPrice,
                    MidPrice = tradeOffer.MidPrice
                });

            TradeOffers = new ObservableCollection<TradeOfferViewModel>(tradeOffersViewModel);
        }
    }
}