using Microsoft.WindowsAzure.Storage.Table;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public interface IChaosTableClient
    {
        CloudTableClient TableClient { get; }

        CloudTable GetTableReference(string table);
    }
}