//
//  UsermapPerson.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Usermap.Data
{
    /// <summary>
    /// Represents usermap person entity.
    /// </summary>
    /// <param name="Username">The username of the user.</param>
    /// <param name="PersonalNumber">The personal number of the user.</param>
    /// <param name="FirstName">The first name of the user.</param>
    /// <param name="LastName">The last name of the user.</param>
    /// <param name="FullName">The full name of the user. Including titles.</param>
    /// <param name="Emails">The list of all visible e-mails of the user.</param>
    /// <param name="PreferredEmail">The e-mail marked as preferred.</param>
    /// <param name="Departments">The departments the user belongs into.</param>
    /// <param name="Rooms">The rooms the user belongs into.</param>
    /// <param name="Phones">The list of all visible phones of the user.</param>
    /// <param name="Roles">Business and technical roles of the user.</param>
    public record UsermapPerson
    (
        [JsonProperty("username")] string Username,
        [JsonProperty("personalNumber")] string PersonalNumber,
        [JsonProperty("firstName")] string FirstName,
        [JsonProperty("lastName")] string LastName,
        [JsonProperty("fullName")] string FullName,
        [JsonProperty("emails")] IReadOnlyList<string> Emails,
        [JsonProperty("preferredEmail")] string PreferredEmail,
        [JsonProperty("departments")] IReadOnlyList<UsermapDepartment> Departments,
        [JsonProperty("rooms")] IReadOnlyList<string> Rooms,
        [JsonProperty("phones")] IReadOnlyList<string> Phones,
        [JsonProperty("roles")] IReadOnlyList<string> Roles
    );
}