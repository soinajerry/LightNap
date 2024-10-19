using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;
using LightNap.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public ProfileController(ILogger<ProfileController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this._logger = logger;
            this._userManager = userManager;
            this._db = db;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<ProfileDto>>> GetProfile()
        {
            var user = await this._db.Users.FindAsync(this.User.GetUserId());
            return ApiResponseDto<ProfileDto>.CreateSuccess(user?.ToLoggedInUserDto());
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponseDto<ProfileDto>>> UpdateProfile(UpdateProfileDto requestDto)
        {
            var user = await this._db.Users.FindAsync(this.User.GetUserId());
            if (user is null) { return this.Unauthorized(); }

            user.UpdateLoggedInUser(requestDto);

            await this._db.SaveChangesAsync();

            return ApiResponseDto<ProfileDto>.CreateSuccess(user.ToLoggedInUserDto());
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ChangePassword(ChangePasswordRequestDto requestDto)
        {
            if (requestDto.NewPassword != requestDto.ConfirmNewPassword) { return ApiResponseDto<bool>.CreateError("New password does not match confirmation password."); }

            ApplicationUser? user = await this._userManager.FindByIdAsync(this.User.GetUserId());
            if (user is null) { return this.BadRequest(); }

            var result = await this._userManager.ChangePasswordAsync(user, requestDto.CurrentPassword, requestDto.NewPassword);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { return ApiResponseDto<bool>.CreateError(result.Errors.Select(item => item.Description).ToArray()); }
                return ApiResponseDto<bool>.CreateError("Unable to change password.");
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

    }
}