using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RestSharp.Serializers;
using Usermap.Caching;
using Usermap.Controllers;

namespace Usermap.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScopedUsermapClientFactory(this IServiceCollection services)
        {
            services
                .AddOptions<UsermapApiOptions>();

            services
                .TryAddScoped<UsermapCacheService>();
            
            services
                .TryAddScoped<UsermapClientFactory>();

            return services;
        }

        public static IServiceCollection AddScopedUsermapApi(this IServiceCollection services,
            Func<IServiceProvider, string> getAccessToken)
        {
            services
                .AddScopedUsermapClientFactory();

            services
                .TryAddScoped<UsermapHttpClient>(p =>
                    p.GetRequiredService<UsermapClientFactory>().CreateClient(getAccessToken(p)));
            
            services
                .TryAddScoped<IUsermapPeopleApi, UsermapPeopleApi>();
            
            return services;
        }

        public static IServiceCollection AddScopedUsermapCaching(this IServiceCollection services)
        {
            services
                .TryAddScoped<UsermapCacheService>();

            return services
                .Replace(ServiceDescriptor.Scoped<IUsermapPeopleApi, CachingUsermapPeopleApi>());
        }
    }
}