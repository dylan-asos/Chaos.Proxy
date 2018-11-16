using System.Web.Http;
using System.Web.Http.Dependencies;
using Chaos.Proxy.WebApi.Handlers;

namespace Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration
{
    public class RouteConfiguration
    {
        public static void Initialise(HttpConfiguration configuration, IDependencyResolver resolver)
        {
            configuration.Routes.MapHttpRoute("chaos.host.create", "chaos/hostmapping",
                new {controller = "Hosts", action = "Post"});

            configuration.Routes.MapHttpRoute("chaos.host.delete", "chaos/hostmapping/{apiKey}",
                new {controller = "Hosts", action = "Delete"});

            configuration.Routes.MapHttpRoute("chaos.config.get", "chaos/configuration/{apiKey}",
                new {controller = "Configuration", action = "Get"});

            configuration.Routes.MapHttpRoute("chaos.config.update", "chaos/configuration",
                new {controller = "Configuration", action = "Post"});

            configuration.Routes.MapHttpRoute("chaos.config.delete", "chaos/configuration/{apiKey}",
                new {controller = "Configuration", action = "Delete"});

            configuration.Routes.MapHttpRoute("chaos.ping", "ping", new {controller = "Ping", action = "Get"});

            configuration.Routes.MapHttpRoute(
                "proxied",
                "{*uri}",
                new {controller = "Default", uri = RouteParameter.Optional},
                null,
                resolver.GetService(typeof(ChaosProxyDelegatingHandler)) as ChaosProxyDelegatingHandler);
        }
    }
}