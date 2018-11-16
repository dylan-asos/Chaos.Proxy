﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.Http;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Handlers
{
    public class ChaosProxyDelegatingHandler : DelegatingHandler
    {
        private readonly ApiHostCache _apiHostCache;

        private readonly IChaosHttpClientFactory _chaosHttpClientFactory;

        private readonly IChaosProxyHostSettings _chaosProxyHostSettings;

        public ChaosProxyDelegatingHandler(IChaosProxyHostSettings chaosProxyHostSettings,
            IChaosHttpClientFactory httpClientFactory, IApiSettingsData apiSettingsData)
        {
            _chaosProxyHostSettings = chaosProxyHostSettings;
            _chaosHttpClientFactory = httpClientFactory;
            _apiHostCache = new ApiHostCache(apiSettingsData);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpRequestMessage proxiedRequest = null;

            try
            {
                var apiHostDetails = await _apiHostCache.GetHost(request.RequestUri);
                if (apiHostDetails == null)
                {
                    return request.CreateErrorResponse(HttpStatusCode.NotFound,
                        $"Host '{request.RequestUri.Host}' not recognised as a valid chaos api");
                }

                var apiToForwardToHostName = apiHostDetails.ForwardApiHostName;

                proxiedRequest = new RequestProxier(request, apiHostDetails, apiToForwardToHostName).CreateProxiedRequest();

                var client = await CreateHttpClientForProxiedRequest(apiToForwardToHostName);

                return await client.SendAsync(proxiedRequest, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                return cancellationToken.IsCancellationRequested
                    ? request.CreateErrorResponse(HttpStatusCode.RequestTimeout,
                        $"Request forward to '{proxiedRequest.RequestUri}' failed, request timed out")
                    : request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                        $"Request forward to '{proxiedRequest.RequestUri}' failed, ");
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                    $"Request forward to '{proxiedRequest.RequestUri}' failed, {ex}");
            }
        }

        private async Task<HttpClient> CreateHttpClientForProxiedRequest(string apiToForwardToHostName)
        {
            var apiConfiguration = await _chaosProxyHostSettings.GetAsync(apiToForwardToHostName);

            var client = _chaosHttpClientFactory.Create(apiToForwardToHostName, apiConfiguration);

            return client;
        }
    }
}