using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Newtonsoft.Json;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services
{
    public class ApiChaosConfigurationSettingsData : IApiChaosConfigurationSettingsData
    {
        private readonly IChaosTableClient _chaosTables;

        public ApiChaosConfigurationSettingsData(IChaosTableClient chaosTables)
        {
            _chaosTables = chaosTables;
        }

        private CloudTable Table => _chaosTables.GetTableReference(TableNames.ApiConfigurations);

        public async Task CreateOrUpdateAsync(ApiHostForwardingSettings forwardSettings, string apiKey,
            ChaosEngine.Configuration.ChaosConfiguration configuration)
        {
            var settings = new ChaosApiSettingsTableEntity
            {
                PartitionKey = forwardSettings.ForwardApiHostName,
                RowKey = forwardSettings.ApiKey,
                Configuration = JsonConvert.SerializeObject(configuration)
            };

            var insertOrReplaceOperation = TableOperation.InsertOrReplace(settings);

            await Table.ExecuteAsync(insertOrReplaceOperation);
        }

        public async Task DeleteAsync(string apiKey)
        {
            var query = new TableQuery<ChaosApiSettingsTableEntity>().Where(
                TableQuery.GenerateFilterCondition(TableConstants.RowKey, QueryComparisons.Equal, apiKey));
            var queryExecutor =
                await Table.ExecuteQueryAsync(query, new TableContinuationToken(), CancellationToken.None);

            var configuration = queryExecutor.FirstOrDefault();

            if (configuration == null) return;

            var deleteOperation = TableOperation.Delete(configuration);
            await Table.ExecuteAsync(deleteOperation);
        }
    }
}