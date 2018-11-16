using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Protocol;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage.Services
{
    public class ApiSettingsData : IApiSettingsData
    {
        private readonly IChaosTableClient _tableClient;

        public ApiSettingsData(IChaosTableClient tableClient)
        {
            _tableClient = tableClient;
        }

        private CloudTable Table => _tableClient.GetTableReference(TableNames.ApiSettings);

        public string GenerateChaosUrl(string sudomain)
        {
            return string.Concat(sudomain, ".", CloudConfigurationManager.GetSetting("ChaosHostUrl")).ToLowerInvariant();
        }

        public async Task<ApiHostForwardingSettings> AddAsync(string subdomain, string forwardHostName, string scheme,
            int port)
        {
            var settings = new ApiHostForwardingSettings
            {
                PartitionKey = GenerateChaosUrl(subdomain), RowKey = Guid.NewGuid().ToString(),
                ForwardApiHostName = forwardHostName.ToLowerInvariant(), Scheme = scheme
            };

            var insertOrReplaceOperation = TableOperation.InsertOrReplace(settings);

            await Table.ExecuteAsync(insertOrReplaceOperation);

            return settings;
        }

        public async Task DeleteAsync(string apiKey)
        {
            var query = new TableQuery<ApiHostForwardingSettings>().Where(
                TableQuery.GenerateFilterCondition(TableConstants.RowKey, QueryComparisons.Equal,
                    apiKey.ToLowerInvariant()));
            var apiDetails = await RunQuery(query);

            if (apiDetails == null) return;

            var deleteOperation = TableOperation.Delete(apiDetails);
            await Table.ExecuteAsync(deleteOperation);
        }

        public async Task<ApiHostForwardingSettings> GetByHostAsync(string host)
        {
            var query = new TableQuery<ApiHostForwardingSettings>().Where(
                TableQuery.GenerateFilterCondition(TableConstants.PartitionKey, QueryComparisons.Equal,
                    host.ToLowerInvariant()));

            return await RunQuery(query);
        }

        public async Task<ApiHostForwardingSettings> GetByApiKeyAsync(string apiKey)
        {
            var query = new TableQuery<ApiHostForwardingSettings>().Where(
                TableQuery.GenerateFilterCondition(TableConstants.RowKey, QueryComparisons.Equal,
                    apiKey.ToLowerInvariant()));

            return await RunQuery(query);
        }

        private async Task<ApiHostForwardingSettings> RunQuery(TableQuery<ApiHostForwardingSettings> query)
        {
            var results = await Table.ExecuteQueryAsync(query, new TableContinuationToken(), CancellationToken.None);

            return results.FirstOrDefault();
        }
    }
}