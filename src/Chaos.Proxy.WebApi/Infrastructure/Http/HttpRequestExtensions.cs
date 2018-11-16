using System;
using System.Net.Http;

namespace Chaos.Proxy.WebApi.Infrastructure.Http
{
    public static class HttpRequestExtensions
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage req, string newUri)
        {
            var clone = new HttpRequestMessage(req.Method, newUri);

            if (req.Method != HttpMethod.Get) clone.Content = req.Content;
            clone.Version = req.Version;

            foreach (var prop in req.Properties) clone.Properties.Add(prop);

            foreach (var header in req.Headers) clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            clone.Headers.Host = new Uri(newUri).Authority;
            return clone;
        }
    }
}