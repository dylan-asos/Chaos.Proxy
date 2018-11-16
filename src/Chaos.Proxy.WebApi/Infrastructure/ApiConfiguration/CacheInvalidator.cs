using System;

namespace Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration
{
    public interface ICacheInvalidator
    {
        event EventHandler<HostConfigurationChangedEventArgs> HostConfigurationChanged;

        void Invalidate(string hostName);
    }

    public class CacheInvalidator : ICacheInvalidator
    {
        public event EventHandler<HostConfigurationChangedEventArgs> HostConfigurationChanged;

        public void Invalidate(string hostName)
        {
            OnHostConfigurationChanged(new HostConfigurationChangedEventArgs {HostName = hostName});
        }

        protected virtual void OnHostConfigurationChanged(HostConfigurationChangedEventArgs e)
        {
            HostConfigurationChanged?.Invoke(this, e);
        }
    }
}