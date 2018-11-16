using System.Net.Http;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public interface IChaoticResponseFactory
    {
        HttpResponseMessage Build(HttpRequestMessage requestMessage, IChaosSettings chaosSettings);
    }
}