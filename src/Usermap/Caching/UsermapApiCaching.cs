using Microsoft.Extensions.Caching.Memory;

namespace Usermap.Caching
{
    internal class UsermapApiCaching
    {
        private readonly MemoryCache _cache;
        
        public UsermapApiCaching(MemoryCache cache)
        {
            _cache = cache;
        }
        
        public bool TryGetFromCache<T>(string identifier, CachePolicy cachePolicy, out T? outEntry)
        {
            if (cachePolicy == CachePolicy.DownloadOnly)
            {
                outEntry = default;
                return false;
            }
            
            bool found = _cache.TryGetValue(identifier, out T? entry);
            
            if (cachePolicy == CachePolicy.CacheOnly && !found)
            {
                throw new CacheEntryNotFoundException();
            }

            outEntry = entry;
            return found;
        }

        public T? SetCache<T>(string identifier, T? data)
        {
            return _cache.Set(identifier, data);
        }
    }
}