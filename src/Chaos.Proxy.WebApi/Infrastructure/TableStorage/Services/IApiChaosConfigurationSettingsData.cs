using System.Threading.Tasks;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services
{
    public interface IApiChaosConfigurationSettingsData
    {
        Task CreateOrUpdateAsync(ApiHostForwardingSettings forwardSettings, string apiKey,
            ChaosEngine.Configuration.ChaosConfiguration configuration);

        Task DeleteAsync(string apiKey);
    }
}