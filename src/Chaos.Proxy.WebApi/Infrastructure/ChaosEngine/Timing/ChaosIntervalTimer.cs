using System;
using System.Timers;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public class ChaosIntervalTimer : DisposableChaosTimer, IChaosIntervalTimer
    {
        public ChaosIntervalTimer(IChaosApiConfiguration configuration)
        {
            InsideChaosWindow = configuration.Enabled;

            if (ShouldNotEnableChaosTimer(configuration))
            {
                return;
            }

            ChaosTimer = new Timer(configuration.ChaosInterval.TotalMilliseconds);
            ChaosTimer.Elapsed += OnTimerElapsed;
            ChaosTimer.Start();
        }

        public bool InsideChaosWindow { get; private set; }

        private static bool ShouldNotEnableChaosTimer(IChaosApiConfiguration configuration)
        {
            if (!configuration.Enabled)
            {
                return true;
            }

            return Convert.ToInt32(configuration.ChaosInterval.TotalMilliseconds) == 0;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            InsideChaosWindow = !InsideChaosWindow;
        }
    }
}