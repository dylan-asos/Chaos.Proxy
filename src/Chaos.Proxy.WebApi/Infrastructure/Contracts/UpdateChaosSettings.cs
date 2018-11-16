using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chaos.Proxy.WebApi.Infrastructure.Contracts
{
    public class UpdateChaosSettings
    {
        public UpdateChaosSettings()
        {
            HttpResponses = new List<UpdateResponseDetails>();
            ResponseFiddles = new List<UpdateResponseFiddle>();
            IgnoreUrls = new List<UpdateUrlPattern>();
        }

        [Required] public string Name { get; set; }

        public int MinResponseDelayTime { get; set; }

        public int MaxResponseDelayTime { get; set; }

        public int PercentageOfChaos { get; set; }

        public int PercentageOfSlowResponses { get; set; }

        public string ResponseTypeMediaType { get; set; }

        public List<UpdateUrlPattern> IgnoreUrls { get; set; }

        public List<UpdateResponseDetails> HttpResponses { get; set; }

        public List<UpdateResponseFiddle> ResponseFiddles { get; set; }
    }
}