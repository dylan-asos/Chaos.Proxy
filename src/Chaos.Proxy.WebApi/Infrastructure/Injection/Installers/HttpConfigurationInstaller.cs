using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling;
using Chaos.Proxy.WebApi.Infrastructure.Injection.Activators;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Installers
{
    public class HttpConfigurationInstaller : IWindsorInstaller
    {
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IHttpControllerActivator>()
                .UsingFactoryMethod(() => new WindsorHttpControllerActivator(container)));

            container.Register(Component.For<IDependencyResolver>()
                .UsingFactoryMethod(() => new WindsorDependencyResolver(container)));

            container.Register(Component.For<IExceptionHandler>().ImplementedBy<GlobalExceptionHandler>());

            container.Register(Component.For<IExceptionLogger>().ImplementedBy<GlobalExceptionLogger>());

            container.Register(Component.For<HttpConfigurationFactory>());

            container.Register(Component.For<HttpConfiguration>()
                .UsingFactory((HttpConfigurationFactory factory) => factory.Create()));
        }
    }
}