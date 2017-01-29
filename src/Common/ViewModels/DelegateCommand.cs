using System;
using System.Windows.Input;

namespace CurrencyExchangeRatesMonitor.Common.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;

        public DelegateCommand(Action execute) : this(null, execute) {}

        public DelegateCommand(Func<bool> canExecute, Action execute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) =>
            canExecute == null
                ? true
                : canExecute();

        public void Execute(object parameter) => execute();
    }
}