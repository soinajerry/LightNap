using LightNap.Core.Interfaces;
using LightNap.WebApi.Extensions;

namespace LightNap.WebApi.Services
{
    /// <summary>
    /// Provides methods to access user-specific information from the HTTP context.
    /// </summary>
    public class WebUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        /// <summary>
        /// Gets the user ID from the HTTP context.
        /// </summary>
        /// <returns>The user ID.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the user ID cannot be retrieved.</exception>
        public string GetUserId()
        {
            return httpContextAccessor.HttpContext?.User.GetUserId() ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the IP address from the HTTP context.
        /// </summary>
        /// <returns>The IP address, or null if it cannot be retrieved.</returns>
        public string? GetIpAddress()
        {
            return httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }
    }

}
