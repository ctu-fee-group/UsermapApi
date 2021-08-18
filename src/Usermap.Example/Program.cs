using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Usermap.Data;

namespace Usermap.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Note: dependency injection is not needed, but it is easier with it
            IServiceProvider services = CreateServices();
            
            UsermapApi api = services.GetRequiredService<UsermapApi>();
            string? accessToken = services
                .GetRequiredService<IOptions<AuthOptions>>()
                .Value.AccessToken;

            if (accessToken == null)
            {
                throw new InvalidOperationException("AccessToken must be set in config");
            }

            // Retrieve authorized usermap api that will remember access token
            AuthorizedUsermapApi authorizedUsermapApi = api.GetAuthorizedApi(accessToken);

            // This could throw if there was error (except 404) and UsermapApiOptions.ThrowOnError was true (that is default)
            UsermapPerson? person = await authorizedUsermapApi.People.GetPersonAsync("username");
            if (person != null)
            {
                Console.WriteLine($"Person {person.FullName} was loaded");
                // You can use person.Roles to list all usermap roles
            }

            // This is obtained from cache
            person = await authorizedUsermapApi.People.GetPersonAsync("username");
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
                
                // Better to add as scoped, but this example does not support scopes
                .AddSingleton<UsermapApi>()
                
                // Add options needed for usermap api
                .Configure<UsermapApiOptions>(config.GetSection("Usermap"))
                
                // Add options with access token, used for example, not part of the library
                .Configure<AuthOptions>(config.GetSection("Auth"))
                .BuildServiceProvider();
        }
    }
}