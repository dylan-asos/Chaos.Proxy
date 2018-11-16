using System;
using System.Timers;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing
{
    public abstract class DisposableChaosTimer : IDisposable
    {
        protected Timer ChaosTimer;

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                ChaosTimer?.Dispose();
            }

            _disposed = true;
        }

        ~DisposableChaosTimer()
        {
            Dispose(false);
        }
    }
}