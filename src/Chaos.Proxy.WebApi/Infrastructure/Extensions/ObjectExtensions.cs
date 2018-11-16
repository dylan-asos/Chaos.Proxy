using Newtonsoft.Json;

namespace Chaos.Proxy.WebApi.Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static string Serialize(this object instance)
        {
            return JsonConvert.SerializeObject(instance, Formatting.Indented);
        }
    }
}