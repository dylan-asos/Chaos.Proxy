using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration
{
    public interface ICacheInvalidator
    {
        Task Invalidate(string hostName);
    }

    public class CacheInvalidator : ICacheInvalidator
    {
        private readonly IApiSettingsData _apiSettings;

        readonly MemoryCache _cache = MemoryCache.Default;

        public CacheInvalidator(IApiSettingsData apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task Invalidate(string hostName)
        {
            var result = await _apiSettings.GetByHostAsync(hostName);

            _cache.Remove(hostName);
            _cache.Remove("client:" + hostName);
            _cache.Remove(result.ApiKey);
        }
    }
}