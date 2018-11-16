using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public class ChaosWebApiActionResult<T> : IHttpActionResult
    {
        private readonly T _content;

        private readonly HttpRequestMessage _httpRequestMessage;

        private readonly HttpStatusCode _resultCode;

        public ChaosWebApiActionResult(HttpRequestMessage httpRequestMessage, HttpStatusCode resultCode, T content)
        {
            _httpRequestMessage = httpRequestMessage;
            _resultCode = resultCode;
            _content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _httpRequestMessage.CreateResponse(_resultCode, _content);
            return Task.FromResult(response);
        }
    }
}