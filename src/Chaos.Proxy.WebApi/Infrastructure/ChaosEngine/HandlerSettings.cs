using System;
using System.Linq;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Configuration;
using Chaos.Proxy.WebApi.Infrastructure.ChaosEngine.Timing;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    public class HandlerSettings : IHandlerSettings
    {
        private readonly IChaosApiConfiguration _configuration;
      
        private readonly IConfigurationRotatationTimer _rotater;

        private readonly object _lockObject = new object();

        private bool _disposed;

        public HandlerSettings(IConfigurationRotatationTimer rotater, IChaosApiConfiguration configuration)
        {
            rotater.RotateConfiguration += OnRotateConfiguration;
            _rotater = rotater;
            _configuration = configuration;
            Current = configuration.ChaosSettings.FirstOrDefault();
        }

        private int CurrentIndex { get; set; }

        public void RotateConfiguration()
        {
            lock (_lockObject)
            {
                CurrentIndex++;

                if (CurrentIndex == _configuration.ChaosSettings.Count)
                {
                    CurrentIndex = 0;
                }

                Current = _configuration.ChaosSettings[CurrentIndex];
            }
        }

        public IChaosSettings Current { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OnRotateConfiguration(object sender, EventArgs eventArgs)
        {
            RotateConfiguration();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                ((DisposableChaosTimer) _rotater)?.Dispose();
            }

            _disposed = true;
        }

        ~HandlerSettings()
        {
            Dispose(false);
        }
    }
}