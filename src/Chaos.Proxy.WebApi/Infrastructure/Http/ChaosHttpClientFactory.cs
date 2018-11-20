using System;
using System.Collections.Concurrent;
using System.Net.Http;
using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;

namespace Chaos.Proxy.WebApi.Infrastructure.Http
{
    public class ChaosHttpClientFactory : IChaosHttpClientFactory
    {
        private readonly ConcurrentDictionary<string, HttpClient> _clients =
            new ConcurrentDictionary<string, HttpClient>();

        public ChaosHttpClientFactory(ICacheInvalidator cacheInvalidator)
        {
            cacheInvalidator.HostConfigurationChanged += OnHostConfigurationChanged;
        }

        public HttpClient Create(string apiToForwardToHostName, ChaosConfiguration apiConfiguration)
        {
            if (_clients.ContainsKey(apiToForwardToHostName))
            {
                return _clients[apiToForwardToHostName];
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

            _clients.TryAdd(apiToForwardToHostName, client);

            return client;
        }

        private void OnHostConfigurationChanged(object sender,
            HostConfigurationChangedEventArgs hostConfigurationChangedEventArgs)
        {
            _clients.TryRemove(hostConfigurationChangedEventArgs.HostName, out _);
        }
    }
}