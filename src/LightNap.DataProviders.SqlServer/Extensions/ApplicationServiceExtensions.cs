using LightNap.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LightNap.Migrations.SqlServer.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddLightNapSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentException("Required connection string 'DefaultConnection' is missing"),
                    sqlOptions => sqlOptions.MigrationsAssembly("LightNap.DataProviders.SqlServer")));

            return services;
        }
    }
}