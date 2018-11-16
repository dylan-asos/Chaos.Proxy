using Castle.Facilities.Startable;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Installers
{
    public class WebApiContainerFactory
    {
        public virtual IWindsorContainer Create()
        {
            var container = new WindsorContainer();

            container.AddFacility<StartableFacility>(f => f.DeferredStart());
            container.AddFacility<TypedFactoryFacility>();

            container.Install(FromAssembly.InThisApplication());

            return container;
        }
    }
}