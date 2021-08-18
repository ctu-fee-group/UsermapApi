using Microsoft.Extensions.Caching.Memory;
namespace Usermap
{
    /// <summary>
    /// Options for usermap api
    /// </summary>
    public class UsermapApiOptions
    {
        public MemoryCacheOptions? CacheOptions { get; set; }
        
        /// <summary>
        /// Url of the API
        /// </summary>
        public string? BaseUrl { get; set; }
    }
}
