using System.Net.Http;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public interface IResponseMediaType
    {
        string GetMediaType(HttpRequestMessage request, IChaosSettings settings);
    }
}