//
//  Program.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Usermap;
using Usermap.Controllers;
using Usermap.Extensions;

namespace Scoped
{
    /// <summary>
    /// The entry point class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point method.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            var services = CreateServices();

            await CallScoped(services, "token1");
            await CallScoped(services, "token2");
        }

        private static async Task CallScoped(IServiceProvider services, string token)
        {
            using var scope = services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            // Set the token
            scopedServices.GetRequiredService<TokenStore>().AccessToken = token;

            var peopleApi = scopedServices.GetRequiredService<IUsermapPeopleApi>();

            // Get person with the specified username
            var person = await peopleApi.GetPersonAsync("username");

            scopedServices.GetRequiredService<ILogger<Program>>()
                .LogInformation("Found person {Person}", person?.FullName);
        }

        private static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                .AddLogging(b => b.AddConsole())
                .AddScoped<TokenStore>()
                .AddMemoryCache()
                .AddUsermapApi
                (
                    (p) => p.GetRequiredService<TokenStore>().AccessToken ?? throw new InvalidOperationException(),
                    lifetime: ServiceLifetime.Scoped
                )
                .AddUsermapCaching(ServiceLifetime.Scoped)
                .BuildServiceProvider();
        }
    }
}