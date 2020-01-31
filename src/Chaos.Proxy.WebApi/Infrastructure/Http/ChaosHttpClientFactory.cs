using System;
using System.Net.Http;
using System.Runtime.Caching;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;

namespace Chaos.Proxy.WebApi.Infrastructure.Http
{
    public class ChaosHttpClientFactory : IChaosHttpClientFactory
    {
        private readonly MemoryCache _memoryCache = MemoryCache.Default;

        public HttpClient Create(string apiKey, ChaosConfiguration apiConfiguration)
        {
            string cacheKey = "client:" + apiKey;

            if (_memoryCache.Contains(cacheKey))
            {
                return (HttpClient)_memoryCache.Get(cacheKey);
            }

            var client =
                HttpClientFactory.Create(
                    new ChaoticDelegatingHandler(
                        new Chance(),
                        new HandlerSettings(new ConfigurationRotationTimer(apiConfiguration), apiConfiguration),
                        new ChaoticResponseFactory(new ResponseMediaType()),
                        new RandomDelay(),
                        new ChaosIntervalTimer(apiConfiguration), new ResponseFiddler(new ResponseMediaType())));

            client.Timeout = TimeSpan.FromSeconds(apiConfiguration.HttpClientTimeoutInSeconds > 0
                ? apiConfiguration.HttpClientTimeoutInSeconds
                : 15);

            _memoryCache.Add(cacheKey, client, DateTimeOffset.UtcNow.AddSeconds(30));

            return client;
        }
    }
}