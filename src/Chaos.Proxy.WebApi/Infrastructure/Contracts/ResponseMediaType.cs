using System.Linq;
using System.Net.Http;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.Contracts
{
    public class ResponseMediaType : IResponseMediaType
    {
        public const string MediaTypeDefault = "application/json";

        public string GetMediaType(HttpRequestMessage request, IChaosSettings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.ResponseTypeMediaType)) return settings.ResponseTypeMediaType;

            var acceptsMediaType = request.Headers.Accept.FirstOrDefault();
            return acceptsMediaType != null ? acceptsMediaType.MediaType : MediaTypeDefault;
        }
    }
}