using LightNap.Core.Identity;

namespace LightNap.WebApi.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);
        string GenerateRefreshToken();
    }
}