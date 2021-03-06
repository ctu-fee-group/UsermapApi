//
//  UsermapPeopleApi.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
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
        public virtual async Task<IReadOnlyList<UsermapPerson>> GetPeopleAsync
        (
            string? query = default,
            string? orderBy = default,
            uint limit = 10,
            uint offset = 0,
            CancellationToken token = default
        )
            => (await _client.GetAsync<IReadOnlyList<UsermapPerson>>
            (
                "people",
                (builder) =>
                {
                    if (query is not null)
                    {
                        builder.AddQuery("query", query);
                    }

                    if (orderBy is not null)
                    {
                        builder.AddQuery("orderBy", orderBy);
                    }

                    builder.AddQuery("limit", limit.ToString());
                    builder.AddQuery("offset", offset.ToString());
                },
                token
            )) ?? new List<UsermapPerson>();

        /// <inheritdoc />
        public virtual async Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default)
        {
            var result = await _client.GetAsync<UsermapPerson?>($"people/{username}", token: token);

            if (result is null)
            {
                _logger.LogWarning("Could not obtain usermap user information({Username})", username);
            }

            return result;
        }

        /// <inheritdoc />
        public virtual async Task<Image?> GetPersonPhotoAsync(string username, CancellationToken token = default)
        {
            var result = await _client.GetImageAsync($"people/{username}/photo", token: token);

            if (result is null)
            {
                _logger.LogWarning("Could not obtain usermap user photo({Username})", username);
            }

            return result;
        }

        /// <inheritdoc />
        public virtual Task<bool> CheckRolesAsync
        (
            string username,
            IEnumerable<string>? hasAllRoles = default,
            IEnumerable<string>? hasAnyRole = default,
            IEnumerable<string>? hasNoneRoles = default,
            CancellationToken token = default
        )
        {
            return _client.HeadAsync
            (
                $"people/{username}/roles",
                (builder) =>
                {
                    if (hasAllRoles is not null)
                    {
                        builder.AddQuery("all", string.Join(',', hasAllRoles));
                    }

                    if (hasAnyRole is not null)
                    {
                        builder.AddQuery("any", string.Join(',', hasAnyRole));
                    }

                    if (hasNoneRoles is not null)
                    {
                        builder.AddQuery("none", string.Join(',', hasNoneRoles));
                    }
                },
                token
            );
        }
    }
}