//
//   UsermapRole.cs
//
//   Copyright (c) Christofel authors. All rights reserved.
//   Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Usermap.Data
{
    /// <summary>
    /// Represents Usermap Role entity.
    /// </summary>
    /// <param name="Code">The code of the role.</param>
    /// <param name="DescriptionCzech">The description of the role in czech language.</param>
    /// <param name="DescriptionEnglish">The description of the role in english.</param>
    /// <param name="OrganizationUnit">The number of the organization unit.</param>
    /// <param name="Type">The type of the role.</param>
    public record UsermapRole
    (
        [property: JsonPropertyName("code")] string Code,
        [property: JsonPropertyName("descriptionCs")]
        string DescriptionCzech,
        [property: JsonPropertyName("descriptionEn")]
        string DescriptionEnglish,
        [property: JsonPropertyName("orgUnit")]
        uint OrganizationUnit,
        [property: JsonPropertyName("type")] UsermapRoleType Type
    );
}