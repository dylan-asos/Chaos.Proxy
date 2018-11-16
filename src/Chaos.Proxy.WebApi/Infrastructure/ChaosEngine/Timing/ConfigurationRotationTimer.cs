using System;
using System.Timers;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public class ConfigurationRotationTimer : DisposableChaosTimer, IConfigurationRotatationTimer
    {
        public ConfigurationRotationTimer(IChaosApiConfiguration configuration)
        {
            if (Convert.ToInt32(configuration.ConfigurationRotationInterval.TotalMilliseconds) == 0)
            {
                return;
            }

            if (configuration.ChaosSettings.Count <= 1)
            {
                return;
            }

            ChaosTimer = new Timer(configuration.ConfigurationRotationInterval.TotalMilliseconds);
            ChaosTimer.Elapsed += OnTimerElapsed;
            ChaosTimer.Start();
        }

        public event EventHandler<EventArgs> RotateConfiguration;

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            RotateConfiguration?.Invoke(this, e);
        }
    }
}