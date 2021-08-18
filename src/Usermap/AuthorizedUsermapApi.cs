using System;
using System.Net.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.NewtonsoftJson;
using Usermap.Caching;
using Usermap.Controllers;

namespace Usermap
{
    /// <summary>
    /// Entity used to interact with usermap API
    /// </summary>
    public class AuthorizedUsermapApi
    {
        private readonly RestClient _client;
        private readonly UsermapApiCaching _caching;
        private readonly ILogger _logger;
        private readonly UsermapApiOptions _options;

        // Controllers
        private UsermapApiPeople? _people;

        
        internal AuthorizedUsermapApi(string accessToken, UsermapApiOptions options, ILogger logger)
        {
            _logger = logger;
            _options = options;
            _client = new RestClient(options.BaseUrl ?? throw new InvalidOperationException("BaseUrl is null"))
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache),
                Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken, "Bearer")
            };

            _caching = new UsermapApiCaching(new MemoryCache(options.CacheOptions ?? new MemoryCacheOptions()));

            _client.UseNewtonsoftJson();
        }

        /// <summary>
        /// Endpoint /people
        /// </summary>
        public UsermapApiPeople People => _people ??= new UsermapApiPeople(_client, _options, _caching, _logger);
    }
}