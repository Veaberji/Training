using Microsoft.Extensions.DependencyInjection;
using MusiciansAPP.API.Configs;
using System;

namespace MusiciansAPP.API.Extensions.Configuration
{
    public static class CorsConfigs
    {
        public static IServiceCollection AddAppCors(
            this IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(uri =>
                            new Uri(uri).Host == AppConfigs.Host)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }));

            return services;
        }
    }
}
