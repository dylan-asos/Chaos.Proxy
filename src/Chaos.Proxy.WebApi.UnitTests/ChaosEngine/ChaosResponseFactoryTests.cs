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
        [SetUp]
        public void Init()
        {
            chaosSettings = new Mock<IChaosSettings>();
            chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);


            responseMediaType = new Mock<IResponseMediaType>();

            requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            chaoticResponseFactory = new ChaoticResponseFactory(responseMediaType.Object);
        }

        private ChaoticResponseFactory chaoticResponseFactory;

        private Mock<IChaosSettings> chaosSettings;

        private Mock<IResponseMediaType> responseMediaType;

        private HttpRequestMessage requestMessage;

        [TestCase("application/json")]
        [TestCase("text/csv")]
        [TestCase("application/xml")]
        public void Returns_Response_With_Specificed_MediaType(string expectedMediaType)
        {
            responseMediaType.Setup(f => f.GetMediaType(requestMessage, chaosSettings.Object))
                .Returns(expectedMediaType);

            var result = chaoticResponseFactory.Build(requestMessage, chaosSettings.Object);

            result.Content.Headers.ContentType.MediaType.Should().Be(expectedMediaType);
        }

        [Test]
        public void Build_Generates_An_HttpResponse_With_Chaos_ReasonPhrase()
        {
            var response = chaoticResponseFactory.Build(requestMessage, chaosSettings.Object);

            response.ReasonPhrase.Should().Be("Chaos");
        }

        [Test]
        public void Returns_Response_From_List_Of_Http_Codes()
        {
            chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);

            var result = chaoticResponseFactory.Build(requestMessage, chaosSettings.Object);

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public void Returns_Response_Payload_From_List()
        {
            chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);

            var result = chaoticResponseFactory.Build(requestMessage, chaosSettings.Object);

            var content = result.Content.ReadAsStringAsync().Result;
            var contentPayload = JsonConvert.DeserializeObject<TestType>(content);

            contentPayload.Test.Should().Be("TestData");
        }
    }
}