using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Handlers
{
    public class ApiHostCache
    {
        private readonly IApiSettingsData _apiSettingsData;

        private readonly MemoryCache _memoryCache = MemoryCache.Default;

        public ApiHostCache(IApiSettingsData apiSettingsData)
        {
            _apiSettingsData = apiSettingsData;
        }

        public async Task<ApiHostForwardingSettings> GetHost(Uri requestUri)
        {
            ApiHostForwardingSettings apiHostDetails;

            var host = requestUri.Host;

            if (!_memoryCache.Contains(host))
            {
                apiHostDetails = await _apiSettingsData.GetByHostAsync(host);
                if (apiHostDetails != null) _memoryCache.Add(host, apiHostDetails, DateTimeOffset.UtcNow.AddSeconds(30));
            }
            else
            {
                apiHostDetails = (ApiHostForwardingSettings)_memoryCache.Get(host);
            }

            return apiHostDetails;
        }
    }
}