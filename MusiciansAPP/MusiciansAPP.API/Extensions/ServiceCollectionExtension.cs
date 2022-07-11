using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusiciansAPP.API.Configs;
using MusiciansAPP.API.Services;
using MusiciansAPP.BL.Services.Albums;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.BL.Services.Tracks;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels;

namespace MusiciansAPP.API.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAppCors(this IServiceCollection services)
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

    public static IServiceCollection AddAppServices(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddSingleton<PagingHelper>();
        services.AddScoped<IErrorHandler, ErrorHandler>();

        services.AddScoped<IArtistsService, ArtistsService>();
        services.AddScoped<IAlbumsService, AlbumsService>();
        services.AddScoped<ITracksService, TracksService>();

        services.AddSingleton<IHttpClient, HttpClientWrapper>();
        services.AddScoped<IWebDataProvider>(p =>
            ActivatorUtilities.CreateInstance<LastFmDataProvider>(p, config["Secrets:LastFmApiKey"]));

        services.AddAutoMapper(cfg =>
            cfg.AddMaps(
                "MusiciansAPP.API",
                "MusiciansAPP.BL",
                "MusiciansAPP.DAL"));

        return services;
    }

    public static IServiceCollection AddDbServices(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(
                config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}