using System;
using System.Net.Http;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;

namespace Chaos.Proxy.WebApi.Infrastructure.Http
{
    public class RequestProxier
    {
        private readonly HttpRequestMessage _request;
        private readonly ApiHostForwardingSettings _hostDetails;
        private readonly string _apiToForwardToHostName;

        public RequestProxier(HttpRequestMessage request, ApiHostForwardingSettings hostDetails, string apiToForwardToHostName)
        {
            _request = request;
            _hostDetails = hostDetails;
            _apiToForwardToHostName = apiToForwardToHostName;
        }

        public HttpRequestMessage CreateProxiedRequest()
        {
            var scheme = string.IsNullOrWhiteSpace(_hostDetails.Scheme) ? _request.RequestUri.Scheme : _hostDetails.Scheme;

            var port = GetTargetPort(_hostDetails, scheme);

            var builder = new UriBuilder(scheme, _apiToForwardToHostName, port, _request.RequestUri.PathAndQuery);

            return _request.Clone(Uri.UnescapeDataString(builder.Uri.ToString()));
        }

        private static int GetTargetPort(ApiHostForwardingSettings hostDetails, string scheme)
        {
            int port;
            if (hostDetails.Port == 0)
                port = scheme == Uri.UriSchemeHttps ? 443 : 80;
            else
                port = hostDetails.Port;

            return port;
        }
    }
}