using LightNap.Core.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    /// <summary>
    /// Service for logged-in business functionality.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController(
        // Suppress CS9113 warning for unused parameter 'publicService'. Remove this if actually using the parameter.
#pragma warning disable CS9113
        IUserService userService
#pragma warning restore CS9113
        ) : ControllerBase
    {
        // Add Web API methods for business data and functionality accessible to a logged in user.
    }
}