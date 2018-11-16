using System.Collections.Generic;

namespace Chaos.Proxy.WebApi.Infrastructure.Contracts
{
    public class UpdateResponseDetails
    {
        public UpdateResponseDetails()
        {
            Payloads = new List<UpdateChaosPayload>();
        }

        public UpdateResponseDetails(List<UpdateChaosPayload> payloads)
        {
            if (payloads != null) Payloads = payloads;
        }

        public int StatusCode { get; set; }

        public List<UpdateChaosPayload> Payloads { get; }
    }
}