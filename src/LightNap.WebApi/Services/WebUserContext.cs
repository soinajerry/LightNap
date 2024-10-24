using LightNap.Core.Interfaces;
using LightNap.WebApi.Extensions;

namespace LightNap.WebApi.Services
{
    public class WebUserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public string GetUserId()
        {
            return httpContextAccessor.HttpContext?.User.GetUserId() ?? throw new InvalidOperationException();
        }

        public string? GetIpAddress()
        {
            return httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
        }
    }

}
