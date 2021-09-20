//
//   UsermapRoleType.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.Serialization;

namespace Usermap.Data
{
    /// <summary>
    /// The type of the <see cref="UsermapRole"/>.
    /// </summary>
    public enum UsermapRoleType
    {
        /// <summary>
        /// The business role.
        /// </summary>
        [EnumMember(Value = "BUSINESS")]
        Business,

        /// <summary>
        /// The technical role.
        /// </summary>
        [EnumMember(Value = "TECHNICAL")]
        Technical,
    }
}