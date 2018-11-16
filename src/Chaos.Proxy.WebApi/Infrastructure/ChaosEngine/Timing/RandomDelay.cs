using System.Threading;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public class RandomDelay : IRandomDelay
    {
        public int DelayFor(int minimumDelayInMilliseconds, int maximumDelayInMilliseconds)
        {
            var delayTime =
                ThreadSafeRandom.PerThreadInstance.Next(minimumDelayInMilliseconds, maximumDelayInMilliseconds);

            Thread.Sleep(delayTime);

            return delayTime;
        }
    }
}