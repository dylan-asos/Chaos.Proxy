using System.Net.Http;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public interface IResponseFiddler
    {
        Task<HttpResponseMessage> Fiddle(HttpResponseMessage responseMessage, IChaosSettings chaosSettings);
    }
}