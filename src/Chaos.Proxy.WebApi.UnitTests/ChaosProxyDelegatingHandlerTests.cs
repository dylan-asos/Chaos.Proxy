using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Handlers;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Http;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests
{
    [TestFixture]
    public sealed class ChaosProxyDelegatingHandlerTests
    {
        private HttpMessageInvoker _invoker;

        private ChaosProxyDelegatingHandler _chaosProxyDelegatingHandler;

        private Mock<IChaosProxyHostSettings> _chaosProxyHostSettings;

        private Mock<IChaosHttpClientFactory> _chaosHttpClientFactory;

        private Mock<IApiSettingsData> _apiSettingsData;

        private HttpClient _httpClient;

        private HttpRequestMessage _message;

        [SetUp]
        public void Init()
        {
            _message = new HttpRequestMessage(HttpMethod.Get, new Uri("http://subdomain.test.com"));

            _chaosProxyHostSettings = new Mock<IChaosProxyHostSettings>();
            _chaosHttpClientFactory = new Mock<IChaosHttpClientFactory>();
            _apiSettingsData = new Mock<IApiSettingsData>();

            _chaosProxyDelegatingHandler = new ChaosProxyDelegatingHandler(_chaosProxyHostSettings.Object,
                _chaosHttpClientFactory.Object, _apiSettingsData.Object);

            _httpClient = new HttpClient(new TestHandler((r, c) => TestHandler.Return200()));

            _chaosHttpClientFactory.Setup(d => d.Create(It.IsAny<string>(), It.IsAny<ChaosConfiguration>()))
                .Returns(_httpClient);

            _invoker = new HttpMessageInvoker(_chaosProxyDelegatingHandler);
        }

        [Test]
        public async Task Creates_Proxied_Request_For_Expected_Domain()
        {
            _apiSettingsData.Setup(d => d.GetByHostAsync("subdomain.test.com"))
                .ReturnsAsync(new ApiHostForwardingSettings {ForwardApiHostName = "forwarded.domain.com"});

            var result = await _invoker.SendAsync(_message, new CancellationToken());

            result.StatusCode.Should().Be(HttpStatusCode.OK);

            _chaosHttpClientFactory.Verify(
                f => f.Create("forwarded.domain.com", It.IsAny<ChaosConfiguration>()), Times.Once);
        }

        [Test]
        public async Task When_Host_Doesnt_Exist_Returns_404()
        {
            var result = await _invoker.SendAsync(_message, new CancellationToken());

            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}