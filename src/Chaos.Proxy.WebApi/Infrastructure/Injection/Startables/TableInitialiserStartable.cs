using System;
using System.Diagnostics.CodeAnalysis;
using Castle.Core;
using Chaos.Proxy.WebApi.Infrastructure.TableStorage;

namespace Chaos.Proxy.WebApi.Infrastructure.Injection.Startables
{
    [ExcludeFromCodeCoverage]
    public class TableInitialiserStartable : IStartable, IDisposable
    {
        private readonly IChaosTableClient _tableClient;

        private bool _isStarted;

        public TableInitialiserStartable(IChaosTableClient tableClient)
        {
            _tableClient = tableClient;
            _isStarted = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            if (_isStarted) return;

            TableInitialisation.EnsureTablesExist(_tableClient);
        }

        public void Stop()
        {
            if (!_isStarted) return;

            _isStarted = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) Stop();
        }
    }
}