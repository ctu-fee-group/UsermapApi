//
//  UsermapDepartment.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json.Serialization;

namespace Usermap.Data
{
    /// <summary>
    /// Represents Usermap Department entity.
    /// </summary>
    /// <param name="Code">The code of the department.</param>
    /// <param name="NameCzech">The name of the department in czech language.</param>
    /// <param name="NameEnglish">The name of the department in english.</param>
    public record UsermapDepartment(
        [property: JsonPropertyName("code")] int Code,
        [property: JsonPropertyName("nameCs")] string NameCzech,
        [property: JsonPropertyName("nameEn")] string NameEnglish
    );
}
