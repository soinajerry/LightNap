using LightNap.WebApi.Interfaces;
using LightNap.WebApi.Services;

namespace LightNap.WebApi.Extensions
{
    /// <summary>
    /// Extension methods for configuring application services.
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        /// Adds application services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}