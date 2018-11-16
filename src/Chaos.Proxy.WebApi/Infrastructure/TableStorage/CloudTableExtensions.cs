using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Chaos.Proxy.WebApi.Infrastructure.TableStorage
{
    public static class CloudTableExtensions
    {
        public static Task<TableQuerySegment<T>> ExecuteQueryAsync<T>(this CloudTable table, TableQuery<T> query,
            TableContinuationToken token, CancellationToken ct = default(CancellationToken))
            where T : ITableEntity, new()
        {
            var ar = table.BeginExecuteQuerySegmented(query, token, null, null);
            ct.Register(ar.Cancel);

            return Task.Factory.FromAsync(ar, table.EndExecuteQuerySegmented<T>);
        }
    }
}