using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Http;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;

namespace Chaos.Proxy.WebApi.Handlers
{
    public class ChaosProxyDelegatingHandler : DelegatingHandler
    {
        private readonly IApiHostCache _apiHostCache;

        private readonly IChaosHttpClientFactory _chaosHttpClientFactory;

        private readonly IChaosProxyHostSettings _chaosProxyHostSettings;


        public ChaosProxyDelegatingHandler(IChaosProxyHostSettings chaosProxyHostSettings,
            IChaosHttpClientFactory httpClientFactory, IApiSettingsData apiSettingsData, IApiHostCache apiHostCache)
        {
            _chaosProxyHostSettings = chaosProxyHostSettings;
            _chaosHttpClientFactory = httpClientFactory;
            _apiHostCache = apiHostCache;
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

                var apiConfiguration = await _chaosProxyHostSettings.GetAsync(apiHostDetails.ApiKey);

                var client = CreateHttpClientForProxiedRequest(apiConfiguration, apiHostDetails.ApiKey);

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

        private HttpClient CreateHttpClientForProxiedRequest(ChaosConfiguration chaosConfiguration, string apiKey)
        {
            var client = _chaosHttpClientFactory.Create(apiKey, chaosConfiguration);

            return client;
        }
    }
}