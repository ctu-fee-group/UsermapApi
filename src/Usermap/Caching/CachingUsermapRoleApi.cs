//
//   CachingUsermapRoleApi.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Usermap.Controllers;
using Usermap.Data;

namespace Usermap.Caching
{
    /// <summary>
    /// Implementation of <see cref="IUsermapRoleApi"/> that caches the data using <see cref="UsermapCacheService"/>.
    /// </summary>
    public class CachingUsermapRoleApi : UsermapRoleApi
    {
        private readonly UsermapCacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingUsermapRoleApi"/> class.
        /// </summary>
        /// <param name="cacheService">The caching service.</param>
        /// <param name="client">The http client.</param>
        /// <param name="logger">The logger.</param>
        public CachingUsermapRoleApi
            (UsermapCacheService cacheService, UsermapHttpClient client, ILogger<CachingUsermapRoleApi> logger)
            : base(client, logger)
        {
            _cacheService = cacheService;
        }

        /// <inheritdoc />
        public override async Task<UsermapRole?> GetRoleAsync(string code, CancellationToken token = default)
        {
            var identifier = $"role/{code}";
            if (_cacheService.TryGetValue<UsermapRole?>(identifier, out var cachedRole))
            {
                return cachedRole;
            }

            return _cacheService.Cache(identifier, await base.GetRoleAsync(code, token));
        }

        /// <inheritdoc />
        public override async Task<IReadOnlyList<string>> GetRoleMembersAsync
            (string code, CancellationToken token = default)
        {
            var identifier = $"roles/{code}/members";
            if (_cacheService.TryGetValue<IReadOnlyList<string>>(identifier, out var cachedMemberUsernames))
            {
                return cachedMemberUsernames ?? new List<string>();
            }

            return _cacheService.Cache(identifier, await base.GetRoleMembersAsync(code, token)) ?? new List<string>();
        }

        /// <inheritdoc />
        public override async Task<IReadOnlyList<UsermapRole>> GetRolesAsync
            (uint limit = 10, uint offset = 0, CancellationToken token = default)
        {
            var identifier = $"roles/all/{limit.ToString()}/{offset.ToString()}";
            if (_cacheService.TryGetValue<IReadOnlyList<UsermapRole>>(identifier, out var cachedRoles))
            {
                return cachedRoles ?? new List<UsermapRole>();
            }

            var roles = await base.GetRolesAsync(limit, offset, token);
            foreach (var role in roles)
            {
                _cacheService.Cache($"roles/{role.Code}", role);
            }

            return _cacheService.Cache(identifier, roles) ?? new List<UsermapRole>();
        }
    }
}