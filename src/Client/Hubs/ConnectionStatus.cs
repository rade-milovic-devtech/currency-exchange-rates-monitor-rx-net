namespace CurrencyExchangeRatesMonitor.Client.Hubs
{
    public enum ConnectionStatus
    {
        Uninitialized,
        Connecting,
        Connected,
        Reconnecting,
        Reconnected,
        Disconnected
    }
}