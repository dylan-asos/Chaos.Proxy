using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chaos.Proxy.WebApi.Handlers;
using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.Http;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;
using Serilog;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Installers
{
    public class ComponentsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            if (store == null) throw new ArgumentNullException(nameof(store));

            container.Register(
                Component.For<IChaosTableClient>().ImplementedBy<ChaosTableClient>().LifestyleTransient());

            container.Register(
                Component.For<IApiHostCache>().ImplementedBy<ApiHostCache>().LifestyleSingleton());

            container.Register(
                Component.For<ICacheInvalidator>().ImplementedBy<CacheInvalidator>().LifestyleSingleton());
            container.Register(Component.For<IChaosHttpClientFactory>().ImplementedBy<ChaosHttpClientFactory>()
                .LifestyleSingleton());

            container.Register(Component.For<ChaosProxyDelegatingHandler>().LifestyleSingleton());

            container.Register(Component.For<IChaosProxyHostSettings>().ImplementedBy<ChaosProxyHostSettings>()
                .LifestyleTransient());

            container.Register(Component.For<IApiSettingsData>().ImplementedBy<ApiSettingsData>().LifestyleTransient());
            container.Register(Component.For<IApiChaosConfigurationSettingsData>()
                .ImplementedBy<ApiChaosConfigurationSettingsData>().LifestyleTransient());

            container.Register(
                Component.For<ILogger>().UsingFactoryMethod(
                    () => new LoggerConfiguration().MinimumLevel.Debug().CreateLogger()));
        }
    }
}