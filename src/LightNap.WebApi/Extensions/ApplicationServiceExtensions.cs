using LightNap.WebApi.Interfaces;
using LightNap.WebApi.Services;

namespace LightNap.WebApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}