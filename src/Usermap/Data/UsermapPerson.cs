//
//  UsermapPerson.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [property: JsonPropertyName("username")] string Username,
        [property: JsonPropertyName("personalNumber")] ulong PersonalNumber,
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("fullName")] string FullName,
        [property: JsonPropertyName("emails")] IReadOnlyList<string> Emails,
        [property: JsonPropertyName("preferredEmail")] string PreferredEmail,
        [property: JsonPropertyName("departments")] IReadOnlyList<UsermapDepartment> Departments,
        [property: JsonPropertyName("rooms")] IReadOnlyList<string> Rooms,
        [property: JsonPropertyName("phones")] IReadOnlyList<string> Phones,
        [property: JsonPropertyName("roles")] IReadOnlyList<string> Roles
    );
}