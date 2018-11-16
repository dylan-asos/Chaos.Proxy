using System;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public class ThreadSafeRandom
    {
        [ThreadStatic] private static Random _perThreadInstance;

        public static Random PerThreadInstance => _perThreadInstance ?? (_perThreadInstance = new Random());
    }
}