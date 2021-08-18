using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using Usermap.Caching;
using Usermap.Data;

namespace Usermap.Controllers
{
    public class UsermapApiPeople
    {
        private readonly RestClient _client;
        private readonly ILogger _logger;
        private readonly UsermapApiCaching _caching;

        internal UsermapApiPeople(RestClient client, UsermapApiCaching caching, ILogger logger)
        {
            _caching = caching;
            _logger = logger;
            _client = client;
        }

        /// <summary>
        /// Obtain person information using /people/{username}
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cachePolicy"></param>
        /// <param name="token"></param>
        /// <returns>Null in case of an error</returns>
        public async Task<UsermapPerson?> GetPersonAsync(string username,
            CachePolicy cachePolicy = CachePolicy.DownloadIfNotAvailable, CancellationToken token = default)
        {
            string identifier = $"people/{username}";
            if (_caching.TryGetFromCache(identifier, cachePolicy, out UsermapPerson? person))
            {
                return person;
            }

            IRestRequest request = new RestRequest("/people/{username}", Method.GET)
                .AddUrlSegment("username", username);

            IRestResponse<UsermapPerson?>? response = await _client.ExecuteAsync<UsermapPerson?>(request, token);
            _caching.SetCache(identifier, response?.Data);

            if (!(response?.IsSuccessful ?? false) || response.Data == null)
            {
                _logger.LogWarning(
                    response?.ErrorException,
                    $"Could not obtain usermap user information({username}): {response?.StatusCode} {response?.ErrorMessage} {response?.Content}"
                );
                return null;
            }

            return response.Data;
        }
    }
}