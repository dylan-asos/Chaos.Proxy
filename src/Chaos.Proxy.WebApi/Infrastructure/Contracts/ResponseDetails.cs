using System.Collections.Generic;

namespace Chaos.Proxy.WebApi.Infrastructure.Contracts
{
    public class ResponseDetails
    {
        public ResponseDetails()
        {
            Payloads = new List<ChaosResponsePayload>();
        }

        public ResponseDetails(List<ChaosResponsePayload> payloads)
        {
            if (payloads != null) Payloads = payloads;
        }

        public int StatusCode { get; set; }

        public List<ChaosResponsePayload> Payloads { get; }
    }
}