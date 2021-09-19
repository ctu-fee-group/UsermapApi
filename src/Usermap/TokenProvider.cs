//
//  TokenProvider.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Usermap
{
    /// <summary>
    /// Provides access tokens for usermap api.
    /// </summary>
    public class TokenProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenProvider"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        public TokenProvider(string accessToken)
        {
            AccessToken = accessToken;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public string AccessToken { get; set; }
    }
}