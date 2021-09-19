//
//  ServiceCollectionExtensions.cs
//
//  Copyright (c) Christofel authors. All rights reserved.
//  Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        /// Registers <see cref="UsermapClientFactory"/> into the collection and adds <see cref="UsermapApiOptions"/>.
        /// </summary>
        /// <param name="services">The collection of the services.</param>
        /// <param name="lifetime">The lifetime for the factory.</param>
        /// <returns>The passed service collection.</returns>
        public static IServiceCollection AddUsermapClientFactory
        (
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton
        )
        {
            services
                .AddOptions<UsermapApiOptions>();

            services
                .TryAdd
                    (ServiceDescriptor.Describe(typeof(UsermapClientFactory), typeof(UsermapClientFactory), lifetime));

            return services;
        }

        /// <summary>
        /// Registers <see cref="IUsermapPeopleApi"/> into the collection.
        /// </summary>
        /// <remarks>
        /// UsermapApi uses <see cref="UsermapClientFactory"/>, it has to be registered in the collection.
        /// If it isn't, it will be registered with the same lifetime as the uesrmap api..
        /// </remarks>
        /// <param name="services">The collection of the services.</param>
        /// <param name="getAccessToken">The function to obtain access token with.</param>
        /// <param name="lifetime">The lifetime for the api.</param>
        /// <returns>The passed service collection.</returns>
        public static IServiceCollection AddUsermapApi
        (
            this IServiceCollection services,
            Func<IServiceProvider, string> getAccessToken,
            ServiceLifetime lifetime = ServiceLifetime.Singleton
        )
        {
            services
                .AddUsermapClientFactory(lifetime);

            services
                .TryAddScoped<UsermapHttpClient>
                (
                    p =>
                        p.GetRequiredService<UsermapClientFactory>().CreateClient(getAccessToken(p))
                );

            services
                .TryAddScoped<IUsermapPeopleApi, UsermapPeopleApi>();

            return services;
        }

        /// <summary>
        /// Replaces the <see cref="IUsermapPeopleApi"/> with implementation of <see cref="CachingUsermapPeopleApi"/>
        /// that caches the results. Registers <see cref="UsermapCacheService"/>.
        /// </summary>
        /// <param name="services">The collection of the services.</param>
        /// <param name="lifetime">The lifetime for the api.</param>
        /// <returns>The passed service collection.</returns>
        public static IServiceCollection AddUsermapCaching
        (
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton
        )
        {
            services
                .TryAddScoped<UsermapCacheService>();

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