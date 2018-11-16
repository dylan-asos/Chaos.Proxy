namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public class Chance : IChance
    {
        public bool Indicated(int percentage)
        {
            return ThreadSafeRandom.PerThreadInstance.Next(100) < percentage;
        }
    }
}