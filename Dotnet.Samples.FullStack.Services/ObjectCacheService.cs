using System;
using System.Runtime.Caching;

namespace Dotnet.Samples.FullStack.Services
{
    public class ObjectCacheService
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        private static readonly CacheItemPolicy Policy = new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTime.Now.AddDays(1)
        };

        internal static T Get<T>(string key) where T : class
        {
            try
            {
                return (T)Cache[key];
            }
            catch
            {
                return null;
            }
        }

        internal static void Add(object value, string key)
        {
            Cache.Add(key, value, Policy);
        }
    }
}
