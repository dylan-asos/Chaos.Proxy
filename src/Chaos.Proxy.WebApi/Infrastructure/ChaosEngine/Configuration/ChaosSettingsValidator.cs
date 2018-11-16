using System;
using System.Collections.Generic;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration
{
    [Serializable]
    public class ChaosSettingsValidator
    {
        internal bool IsValid => ValidationErrors.Count == 0;

        internal List<ChaosSettingError> ValidationErrors { get; private set; } = new List<ChaosSettingError>();

        internal void Validate(IChaosSettings settings)
        {
            ValidationErrors = new List<ChaosSettingError>();

            if (settings.PercentageOfChaos < 0 || settings.PercentageOfChaos > 100)
            {
                ValidationErrors.Add(new ChaosSettingError(settings.Name, "PercentageOfChaos",
                    "Percentage of chaos must be between 0 and 100"));                
            }

            if (settings.PercentageOfSlowResponses < 0 || settings.PercentageOfSlowResponses > 100)
            {
                ValidationErrors.Add(new ChaosSettingError(settings.Name, "PercentageOfSlowResponses",
                    "Percentage of slow responses must be between 0 and 100"));
            }

            if (settings.PercentageOfSlowResponses > 0 && settings.PercentageOfSlowResponses <= 100)
            {
                if (settings.MaxResponseDelayTime <= settings.MinResponseDelayTime)
                    ValidationErrors.Add(new ChaosSettingError(settings.Name, "MaxResponseDelayTime",
                        "Max response time must be greater than min response time"));
            }
        }
    }
}