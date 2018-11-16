using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public class ChaosTableClient : IChaosTableClient
    {
        public ChaosTableClient()
        {
            var storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            TableClient = storageAccount.CreateCloudTableClient();
        }

        public CloudTableClient TableClient { get; }

        public CloudTable GetTableReference(string table)
        {
            return TableClient.GetTableReference(table);
        }
    }
}