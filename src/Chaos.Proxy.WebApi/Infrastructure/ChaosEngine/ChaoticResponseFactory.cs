using System.Net;
using System.Net.Http;
using System.Text;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public class ChaoticResponseFactory : IChaoticResponseFactory
    {
        private readonly IResponseMediaType _responseMediaType;

        public ChaoticResponseFactory(IResponseMediaType responseMediaType)
        {
            _responseMediaType = responseMediaType;
        }

        public HttpResponseMessage Build(HttpRequestMessage requestMessage, IChaosSettings chaosSettings)
        {
            var chaosResponse = chaosSettings.HttpResponses.TakeRandom();
            var response = requestMessage.CreateResponse((HttpStatusCode) chaosResponse.StatusCode);
            response.ReasonPhrase = "Chaos";

            if (chaosResponse.Payloads.Count == 0)
            {
                return response;
            }

            var chaosPayload = chaosResponse.Payloads.TakeRandom();
            response.Content = new StringContent(chaosPayload.Content, Encoding.UTF8,
                _responseMediaType.GetMediaType(requestMessage, chaosSettings));

            return response;
        }
    }
}