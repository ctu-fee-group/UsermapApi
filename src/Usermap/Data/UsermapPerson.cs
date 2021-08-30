using System.Collections.Generic;
using Newtonsoft.Json;

namespace Usermap.Data
{
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
