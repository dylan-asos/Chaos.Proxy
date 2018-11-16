using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public class ResponseFiddler : IResponseFiddler
    {
        private readonly IResponseMediaType _responseMediaType;

        public ResponseFiddler(IResponseMediaType responseMediaType)
        {
            _responseMediaType = responseMediaType;
        }

        public async Task<HttpResponseMessage> Fiddle(HttpResponseMessage responseMessage, IChaosSettings chaosSettings)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();

            foreach (var responseFiddle in chaosSettings.ResponseFiddles)
                content = content.Replace(responseFiddle.Match, responseFiddle.ReplaceMatchingWith);

            var fiddledContent = new StringContent(content, Encoding.UTF8,
                _responseMediaType.GetMediaType(responseMessage.RequestMessage, chaosSettings));

            var cloned = Clone(responseMessage, fiddledContent);

            return cloned;
        }

        private HttpResponseMessage Clone(HttpResponseMessage response, HttpContent content)
        {
            var clone = new HttpResponseMessage(response.StatusCode)
            {
                RequestMessage = response.RequestMessage,
                Version = response.Version,
                ReasonPhrase = response.ReasonPhrase,
                Content = content
            };

            foreach (var header in response.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }
}