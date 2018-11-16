using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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

        public void Validate()
        {
            var errors = new List<ChaosSettingError>();
            foreach (var results in ChaosSettings.Select(settings => settings.Validate())
                .Where(results => results.Count > 0)) errors.InsertRange(0, results);

            if (errors.Count == 0) return;

            var sb = new StringBuilder();
            foreach (var chaosSettingError in errors)
            {
                sb.AppendLine(chaosSettingError.ToString());
            }

            throw new InvalidOperationException($"Configuration errors detected: {Environment.NewLine} {sb}");
        }
    }
}