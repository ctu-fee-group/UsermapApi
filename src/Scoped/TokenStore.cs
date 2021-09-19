//
//   TokenStore.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Scoped
{
    /// <summary>
    /// The storage for access tokens.
    /// </summary>
    public class TokenStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenStore"/> class.
        /// xyz.
        /// </summary>
        public TokenStore()
        {
        }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string? AccessToken { get; set; }
    }
}