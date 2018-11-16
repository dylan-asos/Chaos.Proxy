using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    public class ResponseFiddlerTests
    {
        private readonly ResponseFiddler _responseFiddler;

        public ResponseFiddlerTests()
        {
            _responseFiddler = new ResponseFiddler(new ResponseMediaType());
        }

        [Test]
        public async Task Should_Amend_Matched_Values_With_Fiddled_Response()
        {
            var chaosSettings = new ChaosSettings();

            chaosSettings.ResponseFiddles.Add(new ResponseFiddle
            {
                Match = "http://some-endpoint.com",
                ReplaceMatchingWith = "Fiddled!"
            });

            var responseObject = new TestResponse
            {
                AnotherField = "This-should-be-left-alone",
                Resource = "http://some-endpoint.com"
            };

            var responseMessage = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonConvert.SerializeObject(responseObject)),
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("http://some-api-call.com"))
            };

            var result = await _responseFiddler.Fiddle(responseMessage, chaosSettings);

            var resultContent = await result.Content.ReadAsStringAsync();
            var deserialisedResponse = JsonConvert.DeserializeObject<TestResponse>(resultContent);

            deserialisedResponse.Resource.Should().Be("Fiddled!");
        }

        private class TestResponse
        {
            public string Resource { get; set; }

            public string AnotherField { get; set; }
        }
    }
}