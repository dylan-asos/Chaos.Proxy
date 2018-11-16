namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public class TableInitialisation
    {
        public static void EnsureTablesExist(IChaosTableClient tableClient)
        {
            var apiSettingsTable = tableClient.GetTableReference(TableNames.ApiSettings);
            apiSettingsTable.CreateIfNotExists();

            var configurationSettings = tableClient.GetTableReference(TableNames.ApiConfigurations);
            configurationSettings.CreateIfNotExists();
        }
    }
}