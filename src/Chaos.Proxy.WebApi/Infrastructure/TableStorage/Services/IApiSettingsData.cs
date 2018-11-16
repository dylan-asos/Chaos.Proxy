using System.Threading.Tasks;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services
{
    public interface IApiSettingsData
    {
        string GenerateChaosUrl(string sudomain);

        Task<ApiHostForwardingSettings> AddAsync(string subdomain, string forwardHostName, string scheme, int port);

        Task DeleteAsync(string apiKey);

        Task<ApiHostForwardingSettings> GetByHostAsync(string host);

        Task<ApiHostForwardingSettings> GetByApiKeyAsync(string apiKey);
    }
}