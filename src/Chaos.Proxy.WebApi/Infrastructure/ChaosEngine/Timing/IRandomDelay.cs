namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public interface IRandomDelay
    {
        int DelayFor(int minimumDelayInMilliseconds, int maximumDelayInMilliseconds);
    }
}