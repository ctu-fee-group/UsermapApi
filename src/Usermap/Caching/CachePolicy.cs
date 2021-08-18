namespace Usermap.Caching
{
    public enum CachePolicy
    {
        /// <summary>
        /// Only allow downloading, bypass cache. The result is stored in the cache
        /// </summary>
        DownloadOnly,
        /// <summary>
        /// Downloads only if the entity is not already in cache
        /// </summary>
        DownloadIfNotAvailable,
        /// <summary>
        /// Only allow cache, if the item is not present in the cache, CacheEntryNotFoundException is thrown
        /// </summary>
        CacheOnly
    }
}