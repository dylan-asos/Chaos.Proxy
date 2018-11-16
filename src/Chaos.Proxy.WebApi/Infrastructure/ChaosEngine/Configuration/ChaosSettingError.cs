using System;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration
{
    [Serializable]
    public class ChaosSettingError
    {
        public ChaosSettingError(string configurationName, string configurationItem, string message)
        {
            ConfigurationName = configurationName;
            ConfigurationItem = configurationItem;
            ErrorMessage = message;
        }

        public string ConfigurationName { get; }

        public string ConfigurationItem { get; }

        public string ErrorMessage { get; }

        public override string ToString()
        {
            return $"{ConfigurationName} - {ConfigurationItem}: {ErrorMessage}";
        }
    }
}