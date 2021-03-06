using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusiciansAPP.API.Services.Interfaces;
using MusiciansAPP.API.Services.Logic;
using MusiciansAPP.BL.Services.Albums.Interfaces;
using MusiciansAPP.BL.Services.Albums.Logic;
using MusiciansAPP.BL.Services.Artists.Interfaces;
using MusiciansAPP.BL.Services.Artists.Logic;
using MusiciansAPP.BL.Services.Tracks.Interfaces;
using MusiciansAPP.BL.Services.Tracks.Logic;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.DBDataProvider.Logic;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;

namespace MusiciansAPP.API.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddScoped<IArtistsService, ArtistsService>();
        services.AddScoped<IAlbumsService, AlbumsService>();
        services.AddScoped<ITracksService, TracksService>();
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