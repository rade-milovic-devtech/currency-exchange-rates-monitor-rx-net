using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using log4net;

namespace CurrencyExchangeRatesMonitor.Client.Config
{
    public class LoggingModule : Autofac.Module
    {
        private static void InjectLoggerProperties(object instance)
        {
            var instanceType = instance.GetType();

            var properties = instanceType
              .GetProperties(BindingFlags.Public | BindingFlags.Instance)
              .Where(property =>
                        property.PropertyType == typeof(ILog) &&
                        property.CanWrite &&
                        property.GetIndexParameters().Length == 0);

            foreach (var property in properties)
            {
                property.SetValue(instance, LogManager.GetLogger(instanceType), null);
            }
        }

        private static void OnComponentPreparing(object sender, PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(
                new[]
                {
                    new ResolvedParameter(
                        (parameterInfo, _) => parameterInfo.ParameterType == typeof(ILog),
                        (parameterInfo, _) => LogManager.GetLogger(parameterInfo.Member.DeclaringType)
                    ),
                });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += OnComponentPreparing;

            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }
    }
}