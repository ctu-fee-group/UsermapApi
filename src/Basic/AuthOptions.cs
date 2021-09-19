//
//  AuthOptions.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Basic
{
    /// <summary>
    /// Options for <see cref="Program"/> to init api instances.
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Gets or sets the access token to use for the api requests.
        /// </summary>
        public string? AccessToken { get; set; }
    }
}