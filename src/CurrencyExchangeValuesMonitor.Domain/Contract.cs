using System;

namespace CurrencyExchangeValuesMonitor.Domain
{
    public static class Contract
    {
        private class ContractException : Exception
        {
            public ContractException(String message): base(message) {}
        }

        public static void Requires(Func<bool> predicate, string message)
        {
            if (!predicate())
            {
                throw new ContractException($"Precondition violation: {message}");
            }
        }
    }
}