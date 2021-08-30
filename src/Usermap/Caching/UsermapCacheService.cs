using System;
using Microsoft.Extensions.Caching.Memory;

namespace Usermap.Caching
{
    public class UsermapCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public UsermapCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? Cache<T>(string key, T? value)
        {
            // TODO: move 5 minutes to options dependency
            return _memoryCache.Set(CreateKey(key), value, DateTimeOffset.Now.AddMinutes(5));
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            if (_memoryCache.TryGetValue(CreateKey(key), out var val))
            {
                value = (T?)val;
                return true;
            }

            value = default;
            return false;
        }

        private string CreateKey(string key)
        {
            return "Usermap" + key;
        }
    }
}