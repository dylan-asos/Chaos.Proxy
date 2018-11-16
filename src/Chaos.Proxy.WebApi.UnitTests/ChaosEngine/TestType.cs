using System.Collections.Generic;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;

namespace Chaos.Proxy.WebApi.UnitTests.ChaosEngine
{
    public class TestType
    {
        public string Test { get; set; }
    }

    public class ResponseData
    {
        public static List<ResponseDetails> GetResponses()
        {
            var payloads = new List<ChaosResponsePayload>
                {new ChaosResponsePayload {Content = "{'Test': 'TestData'}", Code = "Test"}};
            return new List<ResponseDetails> {new ResponseDetails(payloads) {StatusCode = 401}};
        }
    }
}