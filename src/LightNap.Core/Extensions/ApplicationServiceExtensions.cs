using LightNap.Core.Interfaces;
using LightNap.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightNap.Core.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddLogToConsoleEmailer(this IServiceCollection services)
        {
            services.AddSingleton<IEmailService, LogToConsoleEmailService>();
            return services;
        }

        public static IServiceCollection AddSmtpEmailer(this IServiceCollection services)
        {
            services.AddSingleton<IEmailService, SmtpEmailService>();
            return services;
        }

    }
}