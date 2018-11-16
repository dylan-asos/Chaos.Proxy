using Microsoft.WindowsAzure.Storage.Table;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public class ApiHostForwardingSettings : TableEntity
    {
        public string ChaosHostName => PartitionKey;

        public string ApiKey => RowKey;

        public string ForwardApiHostName { get; set; }

        public string Scheme { get; set; }

        public int Port { get; set; }
    }
}