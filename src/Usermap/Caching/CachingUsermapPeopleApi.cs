//
//  CachingUsermapPeopleApi.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Usermap.Controllers;
using Usermap.Data;

namespace Usermap.Caching
{
    /// <summary>
    /// Implementation of <see cref="IUsermapPeopleApi"/> that caches the data using <see cref="UsermapCacheService"/>.
    /// </summary>
    public class CachingUsermapPeopleApi : UsermapPeopleApi
    {
        private readonly UsermapCacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingUsermapPeopleApi"/> class.
        /// </summary>
        /// <param name="cacheService">The service for caching.</param>
        /// <param name="client">The http client.</param>
        /// <param name="logger">The logger.</param>
        public CachingUsermapPeopleApi
            (UsermapCacheService cacheService, UsermapHttpClient client, ILogger<UsermapPeopleApi> logger)
            : base(client, logger)
        {
            _cacheService = cacheService;
        }

        /// <inheritdoc/>
        public override async Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default)
        {
            var identifier = $"people/{username}";
            if (_cacheService.TryGetValue<UsermapPerson?>(identifier, out var cachedPerson))
            {
                return cachedPerson;
            }

            return _cacheService.Cache(identifier, await base.GetPersonAsync(username, token));
        }
    }
}