﻿namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public interface IChaosIntervalTimer
    {
        bool InsideChaosWindow { get; }
    }
}