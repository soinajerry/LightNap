using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    /// <summary>
    /// Controller for handling publicly accessible data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PublicController : ControllerBase
    {
        // Add Web API methods for data that is publicly accessible (user doesn't need to be logged in).
    }
}