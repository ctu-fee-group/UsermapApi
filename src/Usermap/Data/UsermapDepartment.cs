//
//  UsermapDepartment.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace Usermap.Data
{
    /// <summary>
    /// Represents Usermap Department entity.
    /// </summary>
    /// <param name="Code">The code of the department.</param>
    /// <param name="NameCzech">The name of the department in czech language.</param>
    /// <param name="NameEnglish">The name of the department in english.</param>
    public record UsermapDepartment(
        [JsonProperty("code")] int Code,
        [JsonProperty("nameCs")] string NameCzech,
        [JsonProperty("nameEn")] string NameEnglish
    );
}
