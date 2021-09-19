//
//  ServiceCollectionExtensions.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Usermap.Caching;
using Usermap.Controllers;

namespace Usermap.Extensions
{
    /// <summary>
    /// A class that contains extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="IUsermapPeopleApi"/> into the collection.
        /// </summary>
        /// <remarks>
        /// Usermap api uses <see cref="IHttpClientFactory"/>.
        /// </remarks>
        /// <param name="serviceCollection">The collection of the services.</param>
        /// <param name="getAccessToken">The function to obtain access token with.</param>
        /// <param name="lifetime">The lifetime for the api.</param>
        /// <returns>The passed service collection.</returns>
        public static IServiceCollection AddUsermapApi
        (
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, string> getAccessToken,
            ServiceLifetime lifetime = ServiceLifetime.Singleton
        )
        {
            var clientBuilder = serviceCollection
                .AddHttpClient
                (
                    "Usermap",
                    (services, client) =>
                    {
                        var token = getAccessToken(services);

                        client.BaseAddress = new Uri
                        (
                            services.GetRequiredService<IOptions<UsermapApiOptions>>().Value.BaseUrl ??
                            throw new InvalidOperationException("Token not found")
                        );

                        if (string.IsNullOrWhiteSpace(token))
                        {
                            throw new InvalidOperationException("The authentication token has to contain something.");
                        }

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                        (
                            "Bearer",
                            token
                        );
                    }
                );

            serviceCollection
                .TryAdd(ServiceDescriptor.Describe(typeof(UsermapHttpClient), typeof(UsermapHttpClient), lifetime));

            serviceCollection
                .TryAddScoped<IUsermapPeopleApi, UsermapPeopleApi>();

            return serviceCollection;
        }

        /// <summary>
        /// Replaces the <see cref="IUsermapPeopleApi"/> with implementation of <see cref="CachingUsermapPeopleApi"/>
        /// that caches the results. Registers <see cref="UsermapCacheService"/>.
        /// </summary>
        /// <param name="services">The collection of the services.</param>
        /// <param name="lifetime">The lifetime for the api.</param>
        /// <param name="configureOptions">The action for configuring the options of the cache.</param>
        /// <returns>The passed service collection.</returns>
        public static IServiceCollection AddUsermapCaching
        (
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton,
            Action<UsermapCacheOptions>? configureOptions = null
        )
        {
            services
                .TryAddScoped<UsermapCacheService>();

            services.Configure<UsermapCacheOptions>(configureOptions);

            return services
                .Replace
                (
                    ServiceDescriptor.Describe
                    (
                        typeof(IUsermapPeopleApi),
                        typeof(CachingUsermapPeopleApi),
                        lifetime
                    )
                );
        }
    }
}