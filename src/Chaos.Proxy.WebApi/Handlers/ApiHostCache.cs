using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Handlers
{
    public interface IApiHostCache
    {
        Task<ApiHostForwardingSettings> GetHost(Uri requestUri);
    }

    public class ApiHostCache : IApiHostCache
    {
        private readonly IApiSettingsData _apiSettingsData;

        private readonly MemoryCache _memoryCache = MemoryCache.Default;

        private readonly ConcurrentDictionary<string, string> _apiKeyLookup = new ConcurrentDictionary<string, string>();

        public ApiHostCache(IApiSettingsData apiSettingsData)
        {
            _apiSettingsData = apiSettingsData;
        }

        public async Task<ApiHostForwardingSettings> GetHost(Uri requestUri)
        {
            var host = requestUri.Host;

            if (_apiKeyLookup.TryGetValue(host, out var cacheKey))
            {
                if (_memoryCache.Contains(cacheKey))
                {
                    return (ApiHostForwardingSettings) _memoryCache.Get(cacheKey);
                }
            }

            var apiHostDetails = await _apiSettingsData.GetByHostAsync(host);
            if (apiHostDetails == null)
            {
                return null;
            }

            cacheKey = $"host-settings:{apiHostDetails.ApiKey}";

            _apiKeyLookup.TryAdd(host, cacheKey);
            _memoryCache.Add(cacheKey, apiHostDetails, DateTimeOffset.UtcNow.AddSeconds(30));

            return apiHostDetails;
        }
    }
}