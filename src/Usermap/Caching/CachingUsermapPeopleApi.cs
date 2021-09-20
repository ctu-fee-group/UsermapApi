//
//  CachingUsermapPeopleApi.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            (UsermapCacheService cacheService, UsermapHttpClient client, ILogger<CachingUsermapPeopleApi> logger)
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

        /// <inheritdoc />
        public override async Task<bool> CheckRolesAsync
        (
            string username,
            IEnumerable<string>? hasAllRoles = default,
            IEnumerable<string>? hasAnyRole = default,
            IEnumerable<string>? hasNoneRoles = default,
            CancellationToken token = default
        )
        {
            // TODO: cache values of returned check roles.
            var personIdentifier = $"people/{username}";
            if (_cacheService.TryGetValue<UsermapPerson?>(personIdentifier, out var cachedPerson))
            {
                if (cachedPerson is null)
                {
                    return false;
                }

                return (hasAllRoles?.All(x => cachedPerson.Roles.Contains(x)) ?? true) &&
                       (hasAnyRole?.Any(x => cachedPerson.Roles.Contains(x)) ?? true) &&
                       (hasNoneRoles?.All(x => !cachedPerson.Roles.Contains(x)) ?? true);
            }

            return await base.CheckRolesAsync(username, hasAllRoles, hasAnyRole, hasNoneRoles, token);
        }

        /// <inheritdoc />
        public override async Task<Image?> GetPersonPhotoAsync(string username, CancellationToken token = default)
        {
            var photoIdentifier = $"people/{username}/photo";
            if (_cacheService.TryGetValue<Image?>(photoIdentifier, out var photo))
            {
                return photo;
            }

            return _cacheService.Cache(photoIdentifier, await base.GetPersonPhotoAsync(username, token));
        }

        /// <inheritdoc />
        public override async Task<IReadOnlyList<UsermapPerson>> GetPeopleAsync
        (
            string? query = default,
            string? orderBy = default,
            uint limit = 10,
            uint offset = 0,
            CancellationToken token = default
        )
        {
            var identifier = $"people/all/{query}/{orderBy}/{limit.ToString()}/{offset.ToString()}";
            if (_cacheService.TryGetValue<IReadOnlyList<UsermapPerson>>(identifier, out var cachedPeople))
            {
                return cachedPeople ?? new List<UsermapPerson>();
            }

            var people = await base.GetPeopleAsync(query, orderBy, limit, offset, token);
            foreach (var person in people)
            {
                _cacheService.Cache($"people/{person.Username}", person);
            }

            return _cacheService.Cache(identifier, people) ?? new List<UsermapPerson>();
        }
    }
}