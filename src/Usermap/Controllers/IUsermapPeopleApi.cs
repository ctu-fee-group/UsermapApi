//
//  IUsermapPeopleApi.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Usermap.Data;

namespace Usermap.Controllers
{
    /// <summary>
    /// Represents api for getting people from the usermap api.
    /// </summary>
    public interface IUsermapPeopleApi
    {
        /// <summary>
        /// Gets the given person by calling GET /people/{username}.
        /// </summary>
        /// <param name="username">The username to get.</param>
        /// <param name="token">The cancellation token for the operation.</param>
        /// <returns>The person obtained.</returns>
        public Task<UsermapPerson?> GetPersonAsync(string username, CancellationToken token = default);
    }
}