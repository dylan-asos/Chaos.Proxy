using System.Threading;
using System.Web.Http;
using Chaos.Proxy.WebApi.Infrastructure.Injection.Installers;
using Microsoft.Owin.BuilderProperties;
using Owin;

namespace Chaos.Proxy.WebApi
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var container = new WebApiContainerFactory().Create();

            var httpConfiguration = container.Resolve<HttpConfiguration>();

            var applicationProperties = new AppProperties(app.Properties);
            var token = applicationProperties.OnAppDisposing;

            if (token != CancellationToken.None) token.Register(container.Dispose);

            app.UseWebApi(httpConfiguration);
        }
    }
}