﻿using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Protocol;
using Newtonsoft.Json;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public class ChaosProxyHostSettings : IChaosProxyHostSettings
    {
        private readonly ConcurrentDictionary<string, ChaosConfiguration> _hostSettings =
            new ConcurrentDictionary<string, ChaosConfiguration>();

        private readonly IChaosTableClient _tableClient;

        public ChaosProxyHostSettings(ICacheInvalidator cacheInvalidator, IChaosTableClient tableClient)
        {
            _tableClient = tableClient;
            cacheInvalidator.HostConfigurationChanged += OnHostConfigurationChanged;
        }

        private CloudTable Table => _tableClient.GetTableReference(TableNames.ApiConfigurations);

        public async Task<ChaosConfiguration> GetAsync(string apiKey)
        {
            if (_hostSettings.ContainsKey(apiKey)) return _hostSettings[apiKey];

            var settings = await LoadConfigurationFromTableStorage(apiKey);
            _hostSettings.TryAdd(apiKey, settings);

            return settings;
        }

        private void OnHostConfigurationChanged(object sender, HostConfigurationChangedEventArgs eventArgs)
        {
            _hostSettings.TryRemove(eventArgs.HostName, out _);
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