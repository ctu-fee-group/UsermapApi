//
//  UsermapApiOptions.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;
using Usermap.Controllers;

namespace Usermap
{
    /// <summary>
    /// Options for the <see cref="UsermapPeopleApi"/>.
    /// </summary>
    public class UsermapApiOptions : IOptionsSnapshot<UsermapApiOptions>
    {
        /// <summary>
        /// Gets or sets whether to throw on any error (except 404). If false, return null.
        /// True by default.
        /// </summary>
        public bool ThrowOnError { get; set; } = true;

        /// <summary>
        /// Gets or sets the base url of the API.
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <inheritdoc />
        public UsermapApiOptions Value => this;

        /// <inheritdoc />
        public UsermapApiOptions Get(string name) => throw new System.NotImplementedException();
    }
}