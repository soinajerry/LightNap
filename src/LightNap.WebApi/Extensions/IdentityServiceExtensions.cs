using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.WebApi.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LightNap.WebApi.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(
                (options) =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration.GetRequiredSetting("Jwt:Issuer"),
                    ValidAudience = configuration.GetRequiredSetting("Jwt:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetRequiredSetting("Jwt:Key")))
                };
            });

            services.AddAuthorizationBuilder()
                .AddPolicy(Policies.RequireAdministratorRole, policy => policy.RequireRole(ApplicationRoles.Administrator.Name!));

            return services;
        }
    }
}