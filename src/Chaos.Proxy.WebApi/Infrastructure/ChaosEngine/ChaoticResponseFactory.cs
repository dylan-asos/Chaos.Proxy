using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
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

            var decodedContent = HttpUtility.HtmlDecode(chaosPayload.Content);

            response.Content = new StringContent(decodedContent, Encoding.UTF8,
                _responseMediaType.GetMediaType(requestMessage, chaosSettings));

            return response;
        }
    }
}