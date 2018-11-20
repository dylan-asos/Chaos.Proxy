using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    [TestFixture]
    public sealed class ChaosDelegatingHandlerTests
    {
        private HttpClient _httpClient;

        private ChaoticDelegatingHandler _chaosDelgatingHandler;

        private HttpRequestMessage _requestMessage;

        private Mock<IChance> _chance;

        private Mock<IHandlerSettings> _handlerSettings;

        private Mock<IChaosSettings> _chaosSettings;

        private Mock<IRandomDelay> _randomDelay;

        private Mock<IChaoticResponseFactory> _responseFactory;

        private Mock<IChaosIntervalTimer> _chaosTimer;

        private Mock<IResponseFiddler> _responseFiddler;

        [SetUp]
        public void Setup()
        {
            _handlerSettings = new Mock<IHandlerSettings>();
            _chance = new Mock<IChance>();

            _chaosSettings = new Mock<IChaosSettings>();
            _chaosSettings.Setup(f => f.HttpResponses).Returns(new List<ResponseDetails>());
            _chaosSettings.Setup(f => f.ResponseFiddles).Returns(new List<ResponseFiddle>());
            _chaosSettings.SetupGet(f => f.Name).Returns("Test");

            _handlerSettings.Setup(f => f.Current).Returns(_chaosSettings.Object);

            _randomDelay = new Mock<IRandomDelay>();
            _responseFactory = new Mock<IChaoticResponseFactory>();
            _responseFiddler = new Mock<IResponseFiddler>();
            _chaosTimer = new Mock<IChaosIntervalTimer>();

            _chaosDelgatingHandler = new ChaoticDelegatingHandler(_chance.Object, _handlerSettings.Object,
                _responseFactory.Object, _randomDelay.Object, _chaosTimer.Object, _responseFiddler.Object)
            {
                InnerHandler =
                    new DummyInnerHandler(
                        (message, token) =>
                            DummyInnerHandler.ReturnDummyOk())
            };

            _httpClient = new HttpClient(_chaosDelgatingHandler);
            _requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com");
            _requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        [TestCase(ChaosResponseHeaders.ConfigurationName, "Test")]
        [TestCase(ChaosResponseHeaders.ChanceMiss, "")]
        public async Task When_Interception_Chance_Is_Not_Indicated_Adds_Expected_Headers(string headerName,
            string expectedValue)
        {
            _chaosTimer.SetupGet(s => s.TimeForChaos).Returns(true);
            _chance.Setup(s => s.Indicated(It.IsAny<int>())).Returns(false);

            var response = await _httpClient.SendAsync(_requestMessage);

            var configHeader = response.Headers.FirstOrDefault(f => f.Key == headerName);
            configHeader.Value.FirstOrDefault().Should().Be(expectedValue);
        }

        [Test]
        public async Task When_Interception_Chance_Is_Indicated_Adds_Chaos_Configuration_Name_Response_Header()
        {
            _chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(false);
            _chaosTimer.SetupGet(f => f.TimeForChaos).Returns(true);
            _chaosSettings.Setup(f => f.Name).Returns("Test");

            var response = await _httpClient.SendAsync(_requestMessage);

            var configHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.ConfigurationName);
            configHeader.Value.FirstOrDefault().Should().Be("Test");
        }

        [Test]
        public async Task When_Interception_Chance_Is_Indicated_And_Delay_Time_Is_Zero_Does_Not_Add_Response_Header()
        {
            _chaosTimer.SetupGet(f => f.TimeForChaos).Returns(true);
            _chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(true);
            _randomDelay.Setup(d => d.DelayFor(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(0);

            var response = await _httpClient.SendAsync(_requestMessage);

            var delayHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.DelayTime);
            delayHeader.Value.Should().BeNull();
        }

        [Test]
        public async Task
            When_Interception_Chance_Is_Indicated_And_Includes_Slow_Response_Then_Adds_Delay_Time_Response_Header_When_Greater_Than_Zero()
        {
            _chaosTimer.SetupGet(f => f.TimeForChaos).Returns(true);
            _chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(true);
            _chaosSettings.Setup(f => f.MinResponseDelayTime).Returns(100);
            _chaosSettings.Setup(f => f.MaxResponseDelayTime).Returns(500);
            _randomDelay.Setup(d => d.DelayFor(100, 500)).ReturnsAsync(300);

            var response = await _httpClient.SendAsync(_requestMessage);

            var delayHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.DelayTime);
            delayHeader.Value.FirstOrDefault().Should().Be("300");
        }

        [Test]
        public async Task When_Interception_Chance_Is_Indicated_And_Includes_Slow_Response_Then_Should_Delay_Response()
        {
            _chaosTimer.SetupGet(f => f.TimeForChaos).Returns(true);
            _chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(true);
            _chaosSettings.Setup(f => f.MinResponseDelayTime).Returns(100);
            _chaosSettings.Setup(f => f.MaxResponseDelayTime).Returns(500);
            _randomDelay.Setup(d => d.DelayFor(100, 500)).Verifiable();

            await _httpClient.SendAsync(_requestMessage);

            _randomDelay.Verify();
        }

        [Test]
        public async Task
            When_Interception_Chance_Is_Indicated_And_Responses_Are_Defined_Returns_Response_From_Factory()
        {
            _responseFactory.Setup(f => f.Build(_requestMessage, _chaosSettings.Object))
                .Returns(new HttpResponseMessage(HttpStatusCode.Accepted) {ReasonPhrase = "Chaos"});
            _chaosTimer.SetupGet(f => f.TimeForChaos).Returns(true);
            _chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(false);
            _chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);

            var response = await _httpClient.SendAsync(_requestMessage);

            _responseFactory.Verify(f => f.Build(_requestMessage, _chaosSettings.Object), Times.Once);
            response.ReasonPhrase.Should().Be("Chaos");
        }

        [Test]
        public async Task
            When_Interception_Chance_Is_Indicated_And_Responses_Are_Not_Defined_Does_Not_Return_Response_From_Factory()
        {
            _chaosTimer.SetupGet(f => f.TimeForChaos).Returns(true);
            _chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(false);

            var response = await _httpClient.SendAsync(_requestMessage);

            _responseFactory.Verify(f => f.Build(_requestMessage, _chaosSettings.Object), Times.Never);
            response.ReasonPhrase.Should().Be("Dummy");
        }

        [Test]
        public async Task When_Interception_Chance_Is_Not_Indicated_Does_Not_Return_Response_From_Factory()
        {
            _chaosTimer.SetupGet(s => s.TimeForChaos).Returns(true);
            _chance.Setup(s => s.Indicated(It.IsAny<int>())).Returns(false);

            await _httpClient.SendAsync(_requestMessage);

            _responseFactory.Verify(f => f.Build(_requestMessage, _chaosSettings.Object), Times.Never);
        }

        [Test]
        public async Task When_No_Current_Setings_Then_Does_Not_Test_For_Chance()
        {
            _chaosTimer.SetupGet(s => s.TimeForChaos).Returns(true);
            _handlerSettings.Setup(f => f.Current).Returns(() => null);

            await _httpClient.SendAsync(_requestMessage);

            _chance.Verify(f => f.Indicated(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task When_Not_Inside_Chaos_Interval_Then_Does_Not_Return_Response_From_Factory()
        {
            _chaosTimer.SetupGet(s => s.TimeForChaos).Returns(false);

            await _httpClient.SendAsync(_requestMessage);

            _responseFactory.Verify(f => f.Build(_requestMessage, _chaosSettings.Object), Times.Never);
        }

        [Test]
        public async Task When_Request_Uri_Is_On_Ignore_Url_List_Then_Does_Not_Apply_Chaos()
        {
            _requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com/customer/profile/customers");

            _chaosTimer.SetupGet(f => f.TimeForChaos).Returns(true);
            _chaosSettings.Setup(f => f.Name).Returns("Test");

            _chaosSettings.Setup(s => s.IgnoreUrlPattern).Returns(new List<string> {"customer/profile"});

            var response = await _httpClient.SendAsync(_requestMessage);

            var ignoredHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.IgnoredUrl);
            ignoredHeader.Value.FirstOrDefault().Should().Be("customer/profile");
        }
    }
}