using Autofac.Integration.SignalR;
using CurrencyExchangeRatesMonitor.Common.Constants;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace CurrencyExchangeRatesMonitor.Server.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            app.UseAutofacMiddleware(App.Container);

            app.MapSignalR(ServerConstants.Path, new HubConfiguration()
            {
                Resolver = new AutofacDependencyResolver(App.Container)
            });
        }
    }
}