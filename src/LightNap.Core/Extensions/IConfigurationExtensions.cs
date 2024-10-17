using Microsoft.Extensions.Configuration;

namespace LightNap.Core.Extensions
{
    public static class IConfigurationExtensions
    {
        public static string GetRequiredSetting(this IConfiguration configuration, string key)
        {
            return configuration[key] ?? throw new ArgumentException($"Required setting '{key}' is missing");
        }
    }
}