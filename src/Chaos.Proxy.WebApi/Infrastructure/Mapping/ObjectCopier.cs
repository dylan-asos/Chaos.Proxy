using Newtonsoft.Json;

namespace Chaos.Proxy.WebApi.Infrastructure.Mapping
{
    public static class ObjectCopier
    {
        public static TR Clone<TR, T>(T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<TR>(serialized);
        }
    }
}