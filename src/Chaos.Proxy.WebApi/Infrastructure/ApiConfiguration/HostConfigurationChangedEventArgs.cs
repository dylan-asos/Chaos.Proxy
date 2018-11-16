using System;

namespace Chaos.Proxy.WebApi.Infrastructure.ApiConfiguration
{
    public class HostConfigurationChangedEventArgs : EventArgs
    {
        public string HostName { get; set; }
    }
}