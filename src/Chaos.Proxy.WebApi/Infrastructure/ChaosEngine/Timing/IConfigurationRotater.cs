using System;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public interface IConfigurationRotatationTimer
    {
        event EventHandler<EventArgs> RotateConfiguration;
    }
}