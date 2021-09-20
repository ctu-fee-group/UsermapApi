//
//  IUsermapPeopleApi.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Usermap.Data;

namespace Usermap.Controllers
{
    /// <summary>
    /// Represents api for getting user information from the usermap api.
    /// </summary>
    public interface IUsermapPeopleApi
    {
        /// <summary>
        /// Gets all of the people paged by calling GET /people.
        /// </summary>
        /// <param name="query">The RSQL query.</param>
        /// <param name="orderBy">A comma-separated list of the fields to order by, optionally prefixed with - for a descending order (default is ascending).</param>
        /// <param name="limit">The maximal number of entries in collection to return.</param>
        /// <param name="offset">The offset of the first entry in collection.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>All of the people matching the query.</returns>
        public Task<IReadOnlyList<UsermapPerson>> GetPeopleAsync
        (
            string? query = default,
            string? orderBy = default,
            uint limit = 10,
            uint offset = 0,
            CancellationToken token = default
        );

        /// <summary>
        /// Gets the given person by calling GET /people/{username}.
        /// </summary>
        /// <param name="username">The username of the person to get.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>The person obtained.</returns>
        public Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default);

        /// <summary>
        /// Gets the given person's photo by calling GET /people/{username}/photo.
        /// </summary>
        /// <param name="username">The username of the person.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>The photo of the person obtained.</returns>
        public Task<Image?> GetPersonPhotoAsync(string username, CancellationToken token = default);

        /// <summary>
        /// Gets the given person's photo by calling GET /people/{username}/photo.
        /// </summary>
        /// <param name="username">The username of the user to check roles for.</param>
        /// <param name="hasAllRoles">Checks that the user has all of the roles specified in this parameter.</param>
        /// <param name="hasAnyRole">Checks that the user has any role of the ones specified in this parameter.</param>
        /// <param name="hasNoneRoles">Checks that the user has none of the roles that are specified in this parameter.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>Whether the query was satisfied. Null in case the user was not found.</returns>
        public Task<bool> CheckRolesAsync
        (
            string username,
            IEnumerable<string>? hasAllRoles = default,
            IEnumerable<string>? hasAnyRole = default,
            IEnumerable<string>? hasNoneRoles = default,
            CancellationToken token = default
        );
    }
}