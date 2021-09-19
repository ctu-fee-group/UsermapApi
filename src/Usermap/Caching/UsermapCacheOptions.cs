//
//   UsermapCacheOptions.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Usermap.Caching
{
    /// <summary>
    /// The options for <see cref="UsermapCacheService"/>.
    /// </summary>
    public class UsermapCacheOptions : IOptions<UsermapCacheOptions>
    {
        /// <summary>
        /// Gets or sets the absolute expiration.
        /// </summary>
        public TimeSpan? AbsoluteExpiration { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets or sets the sliding expiration.
        /// </summary>
        public TimeSpan? SlidingExpiration { get; set; } = TimeSpan.FromSeconds(20);

        /// <summary>
        /// Creates cache entry options for the given type.
        /// </summary>
        /// <typeparam name="T">The type of the entry.</typeparam>
        /// <returns>The options for the entry.</returns>
        public virtual MemoryCacheEntryOptions CreateCacheEntryOptions<T>() => new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = AbsoluteExpiration,
            SlidingExpiration = SlidingExpiration
        };

        /// <inheritdoc />
        public UsermapCacheOptions Value => this;
    }
}