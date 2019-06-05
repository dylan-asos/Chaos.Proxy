using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public interface IChaosProxyHostSettings
    {
        Task<ChaosConfiguration> GetAsync(string apiKey);
    }
}