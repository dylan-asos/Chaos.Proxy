using System.Net.Http;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.Http
{
    public interface IChaosHttpClientFactory
    {
        HttpClient Create(string apiKey, ChaosConfiguration apiConfiguration);
    }
}