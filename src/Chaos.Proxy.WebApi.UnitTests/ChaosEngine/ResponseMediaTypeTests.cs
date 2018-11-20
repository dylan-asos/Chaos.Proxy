using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Hosting;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    [TestFixture]
    public sealed class ResponseMediaTypeTests
    {
        [SetUp]
        public void Init()
        {
            _chaosSettings = new Mock<IChaosSettings>();
            _responseMediaType = new ResponseMediaType();

            _requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com");
            _requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        private ResponseMediaType _responseMediaType;

        private HttpRequestMessage _requestMessage;

        private Mock<IChaosSettings> _chaosSettings;

        [Test]
        public void Returns_Default_Value_When_No_Settings_Could_Be_Retrived()
        {
            var actual = _responseMediaType.GetMediaType(_requestMessage, _chaosSettings.Object);

            actual.Should().Be(ResponseMediaType.MediaTypeDefault);
        }

        [Test]
        public void Returns_Media_Type_From_Request_Message_When_One_Is_Available()
        {
            const string expected = "application/xml";
            _requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(expected));

            var actual = _responseMediaType.GetMediaType(_requestMessage, _chaosSettings.Object);

            actual.Should().Be(expected);
        }

        [Test]
        public void Returns_Media_Type_From_Settings_When_One_Is_Defined()
        {
            const string expected = "application/xml";
            _chaosSettings.Setup(f => f.ResponseTypeMediaType).Returns(expected);

            var actual = _responseMediaType.GetMediaType(_requestMessage, _chaosSettings.Object);

            actual.Should().Be(expected);
        }
    }
}