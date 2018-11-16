using System;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public interface IHandlerSettings : IDisposable
    {
        IChaosSettings Current { get; }

        void RotateConfiguration();
    }
}