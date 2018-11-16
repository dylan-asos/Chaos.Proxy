using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Handlers
{
    public class ApiHostCache
    {
        private readonly IApiSettingsData _apiSettingsData;

        private readonly ConcurrentDictionary<string, ApiHostForwardingSettings> _cachedHosts =
            new ConcurrentDictionary<string, ApiHostForwardingSettings>();

        public ApiHostCache(IApiSettingsData apiSettingsData)
        {
            _apiSettingsData = apiSettingsData;
        }

        public async Task<ApiHostForwardingSettings> GetHost(Uri requestUri)
        {
            ApiHostForwardingSettings apiHostDetails;

            var host = requestUri.Host;

            if (!_cachedHosts.ContainsKey(host))
            {
                apiHostDetails = await _apiSettingsData.GetByHostAsync(host);
                if (apiHostDetails != null) _cachedHosts.TryAdd(host, apiHostDetails);
            }
            else
            {
                apiHostDetails = _cachedHosts[host];
            }

            return apiHostDetails;
        }
    }
}