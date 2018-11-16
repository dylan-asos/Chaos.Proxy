using System.Linq;
using System.Reflection;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Installers
{
    public static class AssemblyDomainServiceName
    {
        static AssemblyDomainServiceName()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            var parts = assemblyName.Split('.');

            BaseNamespace = string.Join(".", parts.Take(3));
        }

        public static string BaseNamespace { get; }
    }
}