//
//   UsermapRoleApi.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Usermap.Data;
using Usermap.Internal;

namespace Usermap.Controllers
{
    /// <inheritdoc />
    public class UsermapRoleApi : IUsermapRoleApi
    {
        private readonly UsermapHttpClient _client;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsermapRoleApi"/> class.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <param name="logger">The logger.</param>
        public UsermapRoleApi(UsermapHttpClient client, ILogger<UsermapRoleApi> logger)
        {
            _logger = logger;
            _client = client;
        }

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<UsermapRole>> GetRolesAsync
            (uint limit = 10, uint offset = 0, CancellationToken token = default)
            => (await _client.GetAsync<IReadOnlyList<UsermapRole>>
               (
                   "roles",
                   (builder) =>
                   {
                       builder.AddQuery("limit", limit.ToString());
                       builder.AddQuery("offset", offset.ToString());
                   },
                   token
               )) ??
               new List<UsermapRole>();

        /// <inheritdoc />
        public virtual Task<UsermapRole?> GetRoleAsync
            (string code, CancellationToken token = default)
            => _client.GetAsync<UsermapRole?>($"roles/{code}", token: token);

        /// <inheritdoc />
        public virtual async Task<IReadOnlyList<string>> GetRoleMembersAsync
            (string code, CancellationToken token = default) => (await _client.GetAsync<RoleMembersResponse?>
                                                                    ($"roles/{code}/members", token: token))?.Members ??
                                                                new List<string>();
    }
}