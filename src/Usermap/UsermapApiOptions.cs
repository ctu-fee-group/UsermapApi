using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Usermap
{
    /// <summary>
    /// Options for usermap api
    /// </summary>
    public class UsermapApiOptions : IOptionsSnapshot<UsermapApiOptions>
    {
        public MemoryCacheOptions? CacheOptions { get; set; }
        
        /// <summary>
        /// Url of the API
        /// </summary>
        public string? BaseUrl { get; set; }

        public UsermapApiOptions Value => this;
        public UsermapApiOptions Get(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}