using Microsoft.WindowsAzure.Storage.Table;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public class ChaosApiSettingsTableEntity : TableEntity
    {
        public string Configuration { get; set; }
    }
}