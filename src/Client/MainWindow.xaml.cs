using System.Windows;
using CurrencyExchangeRatesMonitor.Client.ViewModels;

namespace CurrencyExchangeRatesMonitor.Client
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
            DataContext = mainWindowViewModel;

            InitializeComponent();
        }
    }
}