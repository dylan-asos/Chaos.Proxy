using System.Collections.Generic;

namespace Chaos.Proxy.WebApi.Infrastructure.ChaosEngine
{
    internal static class CollectionExtensions
    {
        public static T TakeRandom<T>(this IList<T> source)
        {
            var randomIndexOfList = ThreadSafeRandom.PerThreadInstance.Next(0, source.Count - 1);

            return source[randomIndexOfList];
        }
    }
}