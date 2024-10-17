using LightNap.Core;
using LightNap.Core.Api;
using LightNap.Core.Extensions;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Identity.Dto.Response;
using LightNap.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _db;

        public ProfileController(ILogger<ProfileController> logger, ApplicationDbContext db)
        {
            this._logger = logger;
            this._db = db;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<LoggedInUserDto>>> GetProfile()
        {
            var user = await this._db.Users.FindAsync(this.User.GetUserId());
            return ApiResponseDto<LoggedInUserDto>.CreateSuccess(user?.ToLoggedInUserDto());
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponseDto<LoggedInUserDto>>> UpdateProfile(UpdateLoggedInUserDto requestDto)
        {
            var user = await this._db.Users.FindAsync(this.User.GetUserId());
            if (user is null) { return this.Unauthorized(); }

            user.UpdateLoggedInUser(requestDto);

            await this._db.SaveChangesAsync();

            return ApiResponseDto<LoggedInUserDto>.CreateSuccess(user.ToLoggedInUserDto());
        }

    }
}