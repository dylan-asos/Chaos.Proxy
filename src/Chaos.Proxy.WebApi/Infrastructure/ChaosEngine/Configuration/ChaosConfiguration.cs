using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration
{
    [Serializable]
    public class ChaosConfiguration : IChaosApiConfiguration
    {
        public int HttpClientTimeoutInSeconds { get; set; }

        public bool Enabled { get; set; }

        public TimeSpan ChaosInterval { get; set; }

        public TimeSpan ConfigurationRotationInterval { get; set; }

        public Collection<ChaosSettings> ChaosSettings { get; } = new Collection<ChaosSettings>();

        public List<ChaosSettingError> ValidationErrors { get; } = new List<ChaosSettingError>();

        public bool IsValid()
        {
            Validate();
            return ValidationErrors.Count == 0;
        }

        public void Validate()
        {
            ValidationErrors.Clear();

            foreach (var results in ChaosSettings.Select(settings => settings.Validate())
                .Where(results => results.Count > 0)) ValidationErrors.InsertRange(0, results);
        }
    }
}