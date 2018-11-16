using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Activators
{
    [ExcludeFromCodeCoverage]
    public class WindsorDependencyResolver : IDependencyResolver, IContainerAccessor
    {
        public WindsorDependencyResolver(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            Container = container;
        }

        public IWindsorContainer Container { get; private set; }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public object GetService(Type serviceType)
        {
            try
            {
                if (Container.Kernel.HasComponent(serviceType)) return Container.Kernel.Resolve(serviceType);

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return (IEnumerable<object>) Container.Kernel.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(Container.Kernel);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (Container == null) return;

            Container.Dispose();
            Container = null;
        }
    }
}