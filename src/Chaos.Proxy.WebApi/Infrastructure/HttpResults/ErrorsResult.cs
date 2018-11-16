using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Chaos.Proxy.WebApi.Infrastructure.ExceptionHandling.Errors;

namespace Chaos.Proxy.WebApi.Infrastructure.HttpResults
{
    public class ErrorsResult : IHttpActionResult
    {
        public ErrorsResult(HttpRequestMessage request, HttpStatusCode httpStatusCode, params ErrorDetails[] errors)
        {
            Request = request;
            HttpStatusCode = httpStatusCode;
            Errors = errors;
        }

        private HttpRequestMessage Request { get; }

        private HttpStatusCode HttpStatusCode { get; }

        private IEnumerable<ErrorDetails> Errors { get; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Request.CreateResponse(HttpStatusCode, Errors));
        }
    }
}