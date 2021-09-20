//
//  Program.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Usermap;
using Usermap.Controllers;
using Usermap.Data;
using Usermap.Extensions;

namespace Basic
{
    /// <summary>
    /// The entry point class for Basic.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point method for Basic.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            // Note: dependency injection is not needed, but it is easier with it
            var services = CreateServices();

            var logger = services.GetRequiredService<ILogger<Program>>();
            var peopleApi = services.GetRequiredService<IUsermapPeopleApi>();
            var roleApi = services.GetRequiredService<IUsermapRoleApi>();

            var checkedRoles = await peopleApi.CheckRolesAsync
            (
                "username",
                hasAllRoles: new List<string>
                    (new[] { "B-13000-STUDENT-PREZENCNI", "B-13000-STUDENT-BAKALAR-PREZENCNI" })
            );
            logger.LogInformation("Has roles: {HasRoles}", checkedRoles);

            // This could throw if there was error (except 404) and UsermapApiOptions.ThrowOnError was true (that is default)
            UsermapPerson? person = await peopleApi.GetPersonAsync("username");
            if (person != null)
            {
                // You can use person.Roles to list all usermap roles
                logger.LogInformation("Person {FullName} was loaded", person.FullName);
            }

            // Gets the user's photo
            Image? photo = await peopleApi.GetPersonPhotoAsync("username");
            if (photo is not null)
            {
                photo.Save("image.png");
            }

            // This is obtained from cache
            person = await peopleApi.GetPersonAsync("username");

            // Gets paged people
            var people = await peopleApi.GetPeopleAsync();
            foreach (var loadedPerson in people)
            {
                logger.LogInformation("Person {FullName} was loaded", loadedPerson.FullName);
            }

            // Gets information about the specified role
            var role = await roleApi.GetRoleAsync("B-13000-STUDENT-BAKALAR");
            logger.LogInformation("Loaded role {Role}", role);

            // Gets paged roles
            var roles = await roleApi.GetRolesAsync();
            foreach (var loadedRole in roles)
            {
                logger.LogInformation("Loaded role {Role}", loadedRole);
            }

            // Gets members of the specified role
            var roleMembers = await roleApi.GetRoleMembersAsync("B-00000-SUMA-OSOBA-CVUT");
            logger.LogInformation
            (
                "Members of the role {Role}: {Members}",
                "B-13000-STUDENT-PROGRAMU-B2656",
                string.Join(", ", roleMembers)
            );
        }

        private static IServiceProvider CreateServices()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            return new ServiceCollection()

                // Logging is needed in case of errors
                .AddLogging
                (
                    builder => builder
                        .AddConsole()
                )
                .AddMemoryCache()
                .AddUsermapApi
                (
                    p =>
                        p.GetRequiredService<IOptions<AuthOptions>>().Value.AccessToken ??
                        throw new InvalidOperationException("Access token cannot be null")
                )
                .AddUsermapCaching()

                // Add options needed for usermap api
                .Configure<UsermapApiOptions>(config.GetSection("Usermap"))

                // Add options with access token, used for example, not part of the library
                .Configure<AuthOptions>(config.GetSection("Auth"))
                .BuildServiceProvider();
        }
    }
}