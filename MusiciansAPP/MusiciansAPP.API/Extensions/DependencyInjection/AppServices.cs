using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusiciansAPP.API.Services.Interfaces;
using MusiciansAPP.API.Services.Logic;
using MusiciansAPP.BL.ArtistsService.Interfaces;
using MusiciansAPP.BL.ArtistsService.Logic;
using MusiciansAPP.DAL.WebDataProvider;

namespace MusiciansAPP.API.Extensions.DependencyInjection
{
    public static class AppServices
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddScoped<IArtistsService, ArtistsService>();
            services.AddScoped<IWebDataProvider>(p =>
                    ActivatorUtilities.CreateInstance<LastFmDataProvider>(p, config["Secrets:LastFmApiKey"]));
            services.AddAutoMapper(cfg =>
                cfg.AddMaps(
                    "MusiciansAPP.API",
                    "MusiciansAPP.BL",
                    "MusiciansAPP.DAL"));
            services.AddScoped<IErrorHandler, ErrorHandler>();

            return services;
        }
    }
}
