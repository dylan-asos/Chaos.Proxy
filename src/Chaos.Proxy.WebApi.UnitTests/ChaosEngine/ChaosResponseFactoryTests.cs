using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    [TestFixture]
    public sealed class ChaoticResponseFactoryTests
    {
        private ChaoticResponseFactory _chaoticResponseFactory;

        private Mock<IChaosSettings> _chaosSettings;

        private Mock<IResponseMediaType> _responseMediaType;

        private HttpRequestMessage _requestMessage;

        [SetUp]
        public void Init()
        {
            _chaosSettings = new Mock<IChaosSettings>();
            _chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);

            _responseMediaType = new Mock<IResponseMediaType>();

            _requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com");
            _requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            _chaoticResponseFactory = new ChaoticResponseFactory(_responseMediaType.Object);
        }

        [TestCase("application/json")]
        [TestCase("text/csv")]
        [TestCase("application/xml")]
        public void Returns_Response_With_Specificed_MediaType(string expectedMediaType)
        {
            _responseMediaType.Setup(f => f.GetMediaType(_requestMessage, _chaosSettings.Object))
                .Returns(expectedMediaType);

            var result = _chaoticResponseFactory.Build(_requestMessage, _chaosSettings.Object);

            result.Content.Headers.ContentType.MediaType.Should().Be(expectedMediaType);
        }

        [Test]
        public void Build_Generates_An_HttpResponse_With_Chaos_ReasonPhrase()
        {
            var response = _chaoticResponseFactory.Build(_requestMessage, _chaosSettings.Object);

            response.ReasonPhrase.Should().Be("Chaos");
        }

        [Test]
        public void Returns_Response_From_List_Of_Http_Codes()
        {
            _chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);

            var result = _chaoticResponseFactory.Build(_requestMessage, _chaosSettings.Object);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void Returns_Response_Payload_From_List()
        {
            _chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);

            var result = _chaoticResponseFactory.Build(_requestMessage, _chaosSettings.Object);

            var content = result.Content.ReadAsStringAsync().Result;
            var contentPayload = JsonConvert.DeserializeObject<TestType>(content);

            contentPayload.Test.Should().Be("TestData");
        }
    }
}