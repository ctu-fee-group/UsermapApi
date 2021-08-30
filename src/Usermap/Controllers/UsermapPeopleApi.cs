using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using Usermap.Caching;
using Usermap.Data;

namespace Usermap.Controllers
{
    public class UsermapPeopleApi : IUsermapPeopleApi
    {
        private readonly UsermapHttpClient _client;
        private readonly ILogger _logger;

        public UsermapPeopleApi(UsermapHttpClient client, ILogger<UsermapPeopleApi> logger)
        {
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
        public virtual async Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default)
        {
            var result = await _client.GetAsync<UsermapPerson?>($"people/{username}");

            if (result is null)
            {
                _logger.LogWarning($"Could not obtain usermap user information({username})");
            }

            return result;
        }
    }
}