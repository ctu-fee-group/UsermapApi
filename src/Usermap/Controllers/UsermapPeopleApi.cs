//
//  UsermapPeopleApi.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Usermap.Data;

namespace Usermap.Controllers
{
    /// <summary>
    /// The api for obtaining people.
    /// </summary>
    public class UsermapPeopleApi : IUsermapPeopleApi
    {
        private readonly UsermapHttpClient _client;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsermapPeopleApi"/> class.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="logger">The logger.</param>
        public UsermapPeopleApi(UsermapHttpClient client, ILogger<UsermapPeopleApi> logger)
        {
            _logger = logger;
            _client = client;
        }

        /// <inheritdoc />
        public virtual async Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default)
        {
            var result = await _client.GetAsync<UsermapPerson?>($"people/{username}");

            if (result is null)
            {
                _logger.LogWarning("Could not obtain usermap user information({Username})", username);
            }

            return result;
        }
    }
}