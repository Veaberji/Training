using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using MusiciansAPP.API.Configs;

namespace MusiciansAPP.API.Extensions.Configuration
{
    public static class ObjectBinding
    {
        public static WebApplicationBuilder BindObjects(
            this WebApplicationBuilder builder)
        {
            builder.Configuration.Bind("AppConfigs", new AppConfigs());

            return builder;
        }
    }
}
