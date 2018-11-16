using System;

namespace Chaos.Proxy.WebApi.Infrastructure.Contracts
{
    [Serializable]
    public class ChaosResponsePayload
    {
        public string Code { get; set; }

        public string Content { get; set; }
    }
}