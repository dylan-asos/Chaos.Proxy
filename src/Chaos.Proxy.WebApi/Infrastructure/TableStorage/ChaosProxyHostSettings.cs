using System;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Newtonsoft.Json;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public class ChaosProxyHostSettings : IChaosProxyHostSettings
    {
        private readonly MemoryCache _memoryCache = MemoryCache.Default;

        private readonly IChaosTableClient _tableClient;

        public ChaosProxyHostSettings(IChaosTableClient tableClient)
        {
            _tableClient = tableClient;
        }

        private CloudTable Table => _tableClient.GetTableReference(TableNames.ApiConfigurations);

        public async Task<ChaosConfiguration> GetAsync(string apiKey)
        {
            string cacheKey = "chaos-settings:" + apiKey;

            if (_memoryCache.Contains(cacheKey))
            {
                return (ChaosConfiguration)_memoryCache.Get(cacheKey);
            }

            var settings = await LoadConfigurationFromTableStorage(apiKey);

            _memoryCache.Add(cacheKey, settings, DateTimeOffset.UtcNow.AddSeconds(30));

            return settings;
        }

        private async Task<ChaosConfiguration> LoadConfigurationFromTableStorage(string apiKey)
        {
            var query = new TableQuery<ChaosApiSettingsTableEntity>().Where(
                TableQuery.GenerateFilterCondition(TableConstants.RowKey, QueryComparisons.Equal, apiKey));

            var queryResults =
                await Table.ExecuteQueryAsync(query, new TableContinuationToken(), CancellationToken.None);
            var results = queryResults.FirstOrDefault();

            return results == null
                ? new ChaosConfiguration()
                : JsonConvert.DeserializeObject<ChaosConfiguration>(results.Configuration);
        }
    }
}