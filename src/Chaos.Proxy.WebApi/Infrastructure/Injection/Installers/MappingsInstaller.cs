using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Chaos.Proxy.WebApi.Infrastructure.Mapping;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Installers
{
    public class MappingsInstaller : IWindsorInstaller
    {
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<HttpResultErrorMapper>().LifestyleSingleton());
        }
    }
}