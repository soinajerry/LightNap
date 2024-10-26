using LightNap.Core.Data.Entities;

namespace LightNap.Core.Interfaces
{
    /// <summary>
    /// Represents a token service interface.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates an access token asynchronously for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the access token is generated.</param>
        /// <returns>The generated access token.</returns>
        Task<string> GenerateAccessTokenAsync(ApplicationUser user);

        /// <summary>
        /// Generates a refresh token.
        /// </summary>
        /// <returns>The generated refresh token.</returns>
        string GenerateRefreshToken();
    }
}