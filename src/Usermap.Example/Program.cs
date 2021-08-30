using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Usermap.Controllers;
using Usermap.Data;
using Usermap.Extensions;

namespace Usermap.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Note: dependency injection is not needed, but it is easier with it
            var services = CreateServices();
            using var scope = services.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var peopleApi = scopedServices.GetRequiredService<IUsermapPeopleApi>();

            // This could throw if there was error (except 404) and UsermapApiOptions.ThrowOnError was true (that is default)
            UsermapPerson? person = await peopleApi.GetPersonAsync("username");
            if (person != null)
            {
                Console.WriteLine($"Person {person.FullName} was loaded");
                // You can use person.Roles to list all usermap roles
            }

            // This is obtained from cache
            person = await peopleApi.GetPersonAsync("username");
        }

        static IServiceProvider CreateServices()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .Build();

            return new ServiceCollection()
                // Logging is needed in case of errors
                .AddLogging(builder => builder
                    .AddConsole())
                .AddScoped<IMemoryCache, MemoryCache>()
                .AddScopedUsermapApi(p =>
                    p.GetRequiredService<IOptions<AuthOptions>>().Value.AccessToken ??
                    throw new InvalidOperationException("Access token cannot be null"))
                .AddScopedUsermapCaching()

                // Add options needed for usermap api
                .Configure<UsermapApiOptions>(config.GetSection("Usermap"))

                // Add options with access token, used for example, not part of the library
                .Configure<AuthOptions>(config.GetSection("Auth"))
                .BuildServiceProvider();
        }
    }
}