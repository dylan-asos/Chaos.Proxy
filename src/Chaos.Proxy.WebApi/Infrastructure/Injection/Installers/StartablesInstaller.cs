﻿using System.Diagnostics.CodeAnalysis;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Installers
{
    public class StartablesInstaller : IWindsorInstaller
    {
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0")]
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IStartable>().WithServiceSelf().LifestyleSingleton());
        }
    }
}