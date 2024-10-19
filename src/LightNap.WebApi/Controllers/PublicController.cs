using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly ILogger _logger;

        public PublicController(ILogger<PublicController> logger)
        {
            this._logger = logger;
        }

        // Add Web API methods for data that is publicly accessible (user doesn't need to be logged in).
    }
}