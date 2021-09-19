//
//  UsermapCacheService.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Caching.Memory;

namespace Usermap.Caching
{
    /// <summary>
    /// Service for caching using <see cref="IMemoryCache"/>.
    /// </summary>
    public class UsermapCacheService
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsermapCacheService"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache to use for caching.</param>
        public UsermapCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Caches the given item.
        /// </summary>
        /// <param name="key">The key identifier to store the value with.</param>
        /// <param name="value">The value to store.</param>
        /// <typeparam name="T">The type of the entity that is being stored.</typeparam>
        /// <returns>The cached value.</returns>
        public T? Cache<T>(string key, T? value) =>
            _memoryCache.Set
                (CreateKey(key), value, DateTimeOffset.Now.AddMinutes(5)); // TODO: move 5 minutes to options dependency

        /// <summary>
        /// Tries to find the value with the given identifier in the storage.
        /// </summary>
        /// <param name="key">The key identifier to look for.</param>
        /// <param name="value">The value that was found.</param>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>Whether entry with the given key was found.</returns>
        public bool TryGetValue<T>(string key, out T? value)
        {
            if (_memoryCache.TryGetValue(CreateKey(key), out var val))
            {
                value = (T?)val;
                return true;
            }

            value = default;
            return false;
        }

        private string CreateKey(string key) => "Usermap" + key;
    }
}