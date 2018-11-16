using System;
using System.Collections.Generic;
using Chaos.Proxy.WebApi.Infrastructure.Contracts;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration
{
    [Serializable]
    public class ChaosSettings : IChaosSettings
    {
        private readonly ChaosSettingsValidator _validator = new ChaosSettingsValidator();

        public ChaosSettings()
        {
            MinResponseDelayTime = 100;
            MaxResponseDelayTime = 500;
            IgnoreUrlPattern = new List<string>();
        }

        public string Name { get; set; }

        public int MinResponseDelayTime { get; set; }

        public int MaxResponseDelayTime { get; set; }

        public int PercentageOfChaos { get; set; }

        public int PercentageOfSlowResponses { get; set; }

        public string ResponseTypeMediaType { get; set; }

        public List<ResponseFiddle> ResponseFiddles { get; } = new List<ResponseFiddle>();

        public List<string> IgnoreUrlPattern { get; }

        public List<ChaosSettingError> Validate()
        {
            _validator.Validate(this);
            return _validator.ValidationErrors;
        }

        public List<ResponseDetails> HttpResponses { get; } = new List<ResponseDetails>();
    }
}