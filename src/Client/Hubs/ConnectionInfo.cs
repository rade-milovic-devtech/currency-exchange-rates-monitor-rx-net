namespace CurrencyExchangeRatesMonitor.Client.Hubs
{
    public class ConnectionInfo
    {
        public ConnectionInfo(string serverAddress, ConnectionStatus connectionStatus)
        {
            ServerAddress = serverAddress;
            ConnectionStatus = connectionStatus;
        }

        public string ServerAddress { get; }
        public ConnectionStatus ConnectionStatus { get; }

        public override string ToString() =>
            $"{nameof(ServerAddress)}: {ServerAddress}, {nameof(ConnectionStatus)}: {ConnectionStatus}";
    }
}