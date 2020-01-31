﻿using System;
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

        private  ConcurrentDictionary<string, string> apiKeyLookup = new ConcurrentDictionary<string, string>();

        public ApiHostCache(IApiSettingsData apiSettingsData)
        {
            _apiSettingsData = apiSettingsData;
        }

        public async Task<ApiHostForwardingSettings> GetHost(Uri requestUri)
        {
            ApiHostForwardingSettings apiHostDetails;

            var host = requestUri.Host;
            string cacheKey = "host-settings:";

            if (apiKeyLookup.TryGetValue(host, out var apiKey))
            {
                cacheKey += apiKey;
            }

            if (!_memoryCache.Contains(cacheKey))
            {
                apiHostDetails = await _apiSettingsData.GetByHostAsync(host);
                if (apiHostDetails != null)
                {
                    _memoryCache.Add(host, apiHostDetails, DateTimeOffset.UtcNow.AddSeconds(30));
                    apiKeyLookup.TryAdd(host, cacheKey);
                }
            }
            else
            {
                apiHostDetails = (ApiHostForwardingSettings)_memoryCache.Get(host);
            }

            return apiHostDetails;
        }
    }
}