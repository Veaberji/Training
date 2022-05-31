using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusiciansAPP.DAL.DBDataProvider;

namespace MusiciansAPP.API.Extensions.DependencyInjection
{
    public static class DbService
    {
        public static IServiceCollection AddDbService(
            this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
