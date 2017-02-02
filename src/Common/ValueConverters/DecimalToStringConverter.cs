using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CurrencyExchangeRatesMonitor.Common.ValueConverters
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class DecimalToStringConverter : IValueConverter
    {
        static DecimalToStringConverter()
        {
            Instance = new DecimalToStringConverter();
        }

        public static DecimalToStringConverter Instance { get; private set; }

        private DecimalToStringConverter() {}

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value == null ? null : ((decimal) value).ToString("#,0.##");

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal returnValue;
            if (decimal.TryParse(value as string, out returnValue))
            {
                return returnValue;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}