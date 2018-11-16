using System.Collections.Generic;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration
{
    public interface IChaosSettings
    {
        string Name { get; set; }

        int MinResponseDelayTime { get; set; }

        int MaxResponseDelayTime { get; set; }

        int PercentageOfChaos { get; set; }

        int PercentageOfSlowResponses { get; set; }

        string ResponseTypeMediaType { get; set; }

        List<ResponseDetails> HttpResponses { get; }

        List<ResponseFiddle> ResponseFiddles { get; }

        List<string> IgnoreUrlPattern { get; }

        List<ChaosSettingError> Validate();
    }
}