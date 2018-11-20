using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    public class DummyInnerHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;

        public DummyInnerHandler()
        {
            _handlerFunc = (r, c) => ReturnDummyOk();
        }

        public DummyInnerHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }

        public static Task<HttpResponseMessage> ReturnDummyOk()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK) {ReasonPhrase = "Dummy"};
            return Task.FromResult(httpResponse);
        }
    }
}