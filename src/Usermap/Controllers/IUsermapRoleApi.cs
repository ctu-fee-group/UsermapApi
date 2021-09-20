//
//   IUsermapRoleApi.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Usermap.Data;

namespace Usermap.Controllers
{
    /// <summary>
    /// Represents api for getting roles information from the usermap api.
    /// </summary>
    public interface IUsermapRoleApi
    {
        /// <summary>
        /// Gets all of the roles paged by calling GET /roles.
        /// </summary>
        /// <param name="limit">The maximal number of entries in collection to return.</param>
        /// <param name="offset">The offset of the first entry in collection.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>All of the people matching the query.</returns>
        public Task<IReadOnlyList<UsermapRole>> GetRolesAsync
        (
            uint limit = 10,
            uint offset = 0,
            CancellationToken token = default
        );

        /// <summary>
        /// Gets information about a specific role by calling GET /roles/{code}.
        /// </summary>
        /// <param name="code">The code of the role.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>All of the people matching the query.</returns>
        public Task<UsermapRole?> GetRoleAsync
        (
            string code,
            CancellationToken token = default
        );

        /// <summary>
        /// Gets role member's usernames by calling GET /roles/{code}.
        /// </summary>
        /// <param name="code">The code of the role.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>All of the people matching the query.</returns>
        public Task<IReadOnlyList<string>> GetRoleMembersAsync
        (
            string code,
            CancellationToken token = default
        );
    }
}