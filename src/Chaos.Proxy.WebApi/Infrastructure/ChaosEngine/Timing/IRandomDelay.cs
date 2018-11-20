using System.Threading.Tasks;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public interface IRandomDelay
    {
        Task<int> DelayFor(int minimumDelayInMilliseconds, int maximumDelayInMilliseconds);
    }
}