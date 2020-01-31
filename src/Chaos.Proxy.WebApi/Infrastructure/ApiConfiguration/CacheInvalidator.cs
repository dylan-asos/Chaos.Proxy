using System.Runtime.Caching;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration
{
    public interface ICacheInvalidator
    {
        void Invalidate(string apiKey);
    }

    public class CacheInvalidator : ICacheInvalidator
    {
        private readonly IApiSettingsData _apiSettings;

        readonly MemoryCache _cache = MemoryCache.Default;

        public CacheInvalidator(IApiSettingsData apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public void Invalidate(string apiKey)
        {
            _cache.Remove("host-settings:" + apiKey);
            _cache.Remove("chaos-settings:" + apiKey);
            _cache.Remove("client:" + apiKey);
        }
    }
}