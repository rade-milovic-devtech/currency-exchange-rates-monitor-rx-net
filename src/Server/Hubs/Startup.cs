using Autofac.Integration.SignalR;
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

            app.MapSignalR("/signalr", new HubConfiguration()
            {
                Resolver = new AutofacDependencyResolver(App.Container)
            });
        }
    }
}