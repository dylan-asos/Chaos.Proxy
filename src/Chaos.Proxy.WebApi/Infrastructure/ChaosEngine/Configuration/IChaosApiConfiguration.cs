using System;
using System.Collections.ObjectModel;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration
{
    public interface IChaosApiConfiguration
    {
        bool Enabled { get; set; }

        TimeSpan ChaosInterval { get; set; }

        TimeSpan ConfigurationRotationInterval { get; set; }

        Collection<ChaosSettings> ChaosSettings { get; }

        void Validate();
    }
}