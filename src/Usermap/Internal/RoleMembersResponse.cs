//
//   RoleMembersResponse.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Usermap.Internal
{
    /// <summary>
    /// The response type for GET /roles/{code}/members
    /// </summary>
    /// <param name="Members">The list of member usernames</param>
    internal record RoleMembersResponse([property: JsonPropertyName("members")] IReadOnlyList<string> Members);
}