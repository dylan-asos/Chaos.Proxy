using System.Threading.Tasks;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public class RandomDelay : IRandomDelay
    {
        public async Task<int> DelayFor(int minimumDelayInMilliseconds, int maximumDelayInMilliseconds)
        {
            var delayTime =
                ThreadSafeRandom.PerThreadInstance.Next(minimumDelayInMilliseconds, maximumDelayInMilliseconds);

            await Task.Delay(delayTime);

            return delayTime;
        }
    }
}