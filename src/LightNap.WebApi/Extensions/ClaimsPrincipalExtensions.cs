using System.Security.Claims;

namespace LightNap.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("JWT did not include required ID claim");
        }
    }
}