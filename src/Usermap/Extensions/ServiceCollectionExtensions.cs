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
        /// <param name="configureClient">The action for configuration of http client.</param>
        /// <param name="lifetime">The lifetime for the api.</param>
        /// <returns>The passed service collection.</returns>
        public static IServiceCollection AddUsermapApi
        (
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, string> getAccessToken,
            Action<IHttpClientBuilder>? configureClient = null,
            ServiceLifetime lifetime = ServiceLifetime.Singleton
        )
        {
            serviceCollection.TryAdd
            (
                ServiceDescriptor.Describe(typeof(TokenProvider), p => new TokenProvider(getAccessToken(p)), lifetime)
            );

            var clientBuilder = serviceCollection
                .AddHttpClient
                (
                    "Usermap",
                    (services, client) =>
                    {
                        var options = services.GetRequiredService<IOptions<UsermapApiOptions>>().Value;
                        client.Timeout = TimeSpan.FromSeconds(options.Timeout);
                        client.BaseAddress = new Uri
                        (
                            options.BaseUrl ??
                            throw new InvalidOperationException("BaseUrl not found")
                        );
                    }
                );
            configureClient?.Invoke(clientBuilder);

            serviceCollection.TryAdd
                (ServiceDescriptor.Describe(typeof(UsermapHttpClient), typeof(UsermapHttpClient), lifetime));

            serviceCollection.TryAdd
                (ServiceDescriptor.Describe(typeof(IUsermapPeopleApi), typeof(UsermapPeopleApi), lifetime));

            serviceCollection.TryAdd
                (ServiceDescriptor.Describe(typeof(IUsermapRoleApi), typeof(UsermapRoleApi), lifetime));

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

            if (configureOptions is not null)
            {
                services.Configure<UsermapCacheOptions>(configureOptions);
            }

            return services
                .Replace
                (
                    ServiceDescriptor.Describe
                    (
                        typeof(IUsermapPeopleApi),
                        typeof(CachingUsermapPeopleApi),
                        lifetime
                    )
                )
                .Replace
                (
                    ServiceDescriptor.Describe
                    (
                        typeof(IUsermapRoleApi),
                        typeof(CachingUsermapRoleApi),
                        lifetime
                    )
                );
        }
    }
}