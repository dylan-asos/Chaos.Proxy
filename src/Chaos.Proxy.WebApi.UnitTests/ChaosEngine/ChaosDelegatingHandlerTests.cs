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
        [SetUp]
        public void Setup()
        {
            handlerSettings = new Mock<IHandlerSettings>();
            chance = new Mock<IChance>();

            chaosSettings = new Mock<IChaosSettings>();
            chaosSettings.Setup(f => f.HttpResponses).Returns(new List<ResponseDetails>());
            chaosSettings.Setup(f => f.ResponseFiddles).Returns(new List<ResponseFiddle>());
            chaosSettings.SetupGet(f => f.Name).Returns("Test");

            handlerSettings.Setup(f => f.Current).Returns(chaosSettings.Object);

            randomDelay = new Mock<IRandomDelay>();
            responseFactory = new Mock<IChaoticResponseFactory>();
            responseFiddler = new Mock<IResponseFiddler>();
            chaosTimer = new Mock<IChaosIntervalTimer>();

            chaosDelgatingHandler = new ChaoticDelegatingHandler(chance.Object, handlerSettings.Object,
                responseFactory.Object, randomDelay.Object, chaosTimer.Object, responseFiddler.Object)
            {
                InnerHandler =
                    new DummyInnerHandler(
                        (message, token) =>
                            DummyInnerHandler.ReturnDummyOk())
            };

            httpClient = new HttpClient(chaosDelgatingHandler);
            requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com");
            requestMessage.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
        }

        private HttpClient httpClient;

        private ChaoticDelegatingHandler chaosDelgatingHandler;

        private HttpRequestMessage requestMessage;

        private Mock<IChance> chance;

        private Mock<IHandlerSettings> handlerSettings;

        private Mock<IChaosSettings> chaosSettings;

        private Mock<IRandomDelay> randomDelay;

        private Mock<IChaoticResponseFactory> responseFactory;

        private Mock<IChaosIntervalTimer> chaosTimer;

        private Mock<IResponseFiddler> responseFiddler;

        [TestCase(ChaosResponseHeaders.ConfigurationName, "Test")]
        [TestCase(ChaosResponseHeaders.ChanceMiss, "")]
        public async Task When_Interception_Chance_Is_Not_Indicated_Adds_Expected_Headers(string headerName,
            string expectedValue)
        {
            chaosTimer.SetupGet(s => s.InsideChaosWindow).Returns(true);
            chance.Setup(s => s.Indicated(It.IsAny<int>())).Returns(false);

            var response = await httpClient.SendAsync(requestMessage);

            var configHeader = response.Headers.FirstOrDefault(f => f.Key == headerName);
            configHeader.Value.FirstOrDefault().Should().Be(expectedValue);
        }

        [Test]
        public async Task When_Interception_Chance_Is_Indicated_Adds_Chaos_Configuration_Name_Response_Header()
        {
            chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(false);
            chaosTimer.SetupGet(f => f.InsideChaosWindow).Returns(true);
            chaosSettings.Setup(f => f.Name).Returns("Test");

            var response = await httpClient.SendAsync(requestMessage);

            var configHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.ConfigurationName);
            configHeader.Value.FirstOrDefault().Should().Be("Test");
        }

        [Test]
        public async Task When_Interception_Chance_Is_Indicated_And_Delay_Time_Is_Zero_Does_Not_Add_Response_Header()
        {
            chaosTimer.SetupGet(f => f.InsideChaosWindow).Returns(true);
            chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(true);
            randomDelay.Setup(d => d.DelayFor(It.IsAny<int>(), It.IsAny<int>())).Returns(0);

            var response = await httpClient.SendAsync(requestMessage);

            var delayHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.DelayTime);
            delayHeader.Value.Should().BeNull();
        }

        [Test]
        public async Task
            When_Interception_Chance_Is_Indicated_And_Includes_Slow_Response_Then_Adds_Delay_Time_Response_Header_When_Greater_Than_Zero()
        {
            chaosTimer.SetupGet(f => f.InsideChaosWindow).Returns(true);
            chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(true);
            chaosSettings.Setup(f => f.MinResponseDelayTime).Returns(100);
            chaosSettings.Setup(f => f.MaxResponseDelayTime).Returns(500);
            randomDelay.Setup(d => d.DelayFor(100, 500)).Returns(300);

            var response = await httpClient.SendAsync(requestMessage);

            var delayHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.DelayTime);
            delayHeader.Value.FirstOrDefault().Should().Be("300");
        }

        [Test]
        public async Task When_Interception_Chance_Is_Indicated_And_Includes_Slow_Response_Then_Should_Delay_Response()
        {
            chaosTimer.SetupGet(f => f.InsideChaosWindow).Returns(true);
            chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(true);
            chaosSettings.Setup(f => f.MinResponseDelayTime).Returns(100);
            chaosSettings.Setup(f => f.MaxResponseDelayTime).Returns(500);
            randomDelay.Setup(d => d.DelayFor(100, 500)).Verifiable();

            await httpClient.SendAsync(requestMessage);

            randomDelay.Verify();
        }

        [Test]
        public async Task
            When_Interception_Chance_Is_Indicated_And_Responses_Are_Defined_Returns_Response_From_Factory()
        {
            responseFactory.Setup(f => f.Build(requestMessage, chaosSettings.Object))
                .Returns(new HttpResponseMessage(HttpStatusCode.Accepted) {ReasonPhrase = "Chaos"});
            chaosTimer.SetupGet(f => f.InsideChaosWindow).Returns(true);
            chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(false);
            chaosSettings.Setup(f => f.HttpResponses).Returns(ResponseData.GetResponses);

            var response = await httpClient.SendAsync(requestMessage);

            responseFactory.Verify(f => f.Build(requestMessage, chaosSettings.Object), Times.Once);
            response.ReasonPhrase.Should().Be("Chaos");
        }

        [Test]
        public async Task
            When_Interception_Chance_Is_Indicated_And_Responses_Are_Not_Defined_Does_Not_Return_Response_From_Factory()
        {
            chaosTimer.SetupGet(f => f.InsideChaosWindow).Returns(true);
            chance.SetupSequence(s => s.Indicated(It.IsAny<int>())).Returns(true).Returns(false);

            var response = await httpClient.SendAsync(requestMessage);

            responseFactory.Verify(f => f.Build(requestMessage, chaosSettings.Object), Times.Never);
            response.ReasonPhrase.Should().Be("Dummy");
        }

        [Test]
        public async Task When_Interception_Chance_Is_Not_Indicated_Does_Not_Return_Response_From_Factory()
        {
            chaosTimer.SetupGet(s => s.InsideChaosWindow).Returns(true);
            chance.Setup(s => s.Indicated(It.IsAny<int>())).Returns(false);

            await httpClient.SendAsync(requestMessage);

            responseFactory.Verify(f => f.Build(requestMessage, chaosSettings.Object), Times.Never);
        }

        [Test]
        public async Task When_No_Current_Setings_Then_Does_Not_Test_For_Chance()
        {
            chaosTimer.SetupGet(s => s.InsideChaosWindow).Returns(true);
            handlerSettings.Setup(f => f.Current).Returns(() => null);

            await httpClient.SendAsync(requestMessage);

            chance.Verify(f => f.Indicated(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public async Task When_Not_Inside_Chaos_Interval_Then_Does_Not_Return_Response_From_Factory()
        {
            chaosTimer.SetupGet(s => s.InsideChaosWindow).Returns(false);

            await httpClient.SendAsync(requestMessage);

            responseFactory.Verify(f => f.Build(requestMessage, chaosSettings.Object), Times.Never);
        }

        [Test]
        public async Task When_Request_Uri_Is_On_Ignore_Url_List_Then_Does_Not_Apply_Chaos()
        {
            requestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.test.com/customer/profile/customers");

            chaosTimer.SetupGet(f => f.InsideChaosWindow).Returns(true);
            chaosSettings.Setup(f => f.Name).Returns("Test");

            chaosSettings.Setup(s => s.IgnoreUrlPattern).Returns(new List<string> {"customer/profile"});

            var response = await httpClient.SendAsync(requestMessage);

            var ignoredHeader = response.Headers.FirstOrDefault(f => f.Key == ChaosResponseHeaders.IgnoredUrl);
            ignoredHeader.Value.FirstOrDefault().Should().Be("customer/profile");
        }
    }
}