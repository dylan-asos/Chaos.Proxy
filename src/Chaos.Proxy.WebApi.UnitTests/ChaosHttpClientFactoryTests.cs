using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Http;
using FluentAssertions;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests
{
    [TestFixture]
    public sealed class ChaosHttpClientFactoryTests
    {
        [SetUp]
        public void Init()
        {
            chaosHttpClientFactory = new ChaosHttpClientFactory(new CacheInvalidator());
        }

        private ChaosHttpClientFactory chaosHttpClientFactory;

        [Test]
        public void Creates_An_Http_Client()
        {
            var result = chaosHttpClientFactory.Create("test.com", new ChaosConfiguration());

            result.Should().NotBeNull();
        }
    }
}