namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public interface IChance
    {
        bool Indicated(int percentage);
    }
}