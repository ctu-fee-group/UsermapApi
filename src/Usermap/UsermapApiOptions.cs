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
        /// Whether to throw on any error (except 404). If false, return null.
        /// True by default.
        /// </summary>
        public bool ThrowOnError { get; set; } = true;
        
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