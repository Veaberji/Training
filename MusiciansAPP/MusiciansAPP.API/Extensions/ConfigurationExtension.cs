using Microsoft.Extensions.Configuration;
using MusiciansAPP.API.Configs;

namespace MusiciansAPP.API.Extensions;

public static class ConfigurationExtension
{
    public static IConfiguration BindObjects(this IConfiguration config)
    {
        config.Bind("AppConfigs", new AppConfigs());

        return config;
    }
}