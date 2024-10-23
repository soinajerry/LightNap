using LightNap.Core.Api;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;
using LightNap.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the profile of the current user.
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponseDto{T}"/> containing the profile of the current user.
        /// </returns>
        /// <response code="200">Returns the profile of the current user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ApiResponseDto<ProfileDto>>> GetProfile()
        {
            return await profileService.GetProfile(this.User.GetUserId());
        }

        /// <summary>
        /// Updates the profile of the current user.
        /// </summary>
        /// <param name="requestDto">The updated profile information.</param>
        /// <returns>
        /// An <see cref="ApiResponseDto{T}"/> containing the updated profile of the current user.
        /// </returns>
        /// <response code="200">Returns the updated profile of the current user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDto>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<ProfileDto>>> UpdateProfile(UpdateProfileDto requestDto)
        {
            return await profileService.UpdateProfileAsync(this.User.GetUserId(), requestDto);
        }

        /// <summary>
        /// Changes the password of the current user.
        /// </summary>
        /// <param name="requestDto">The password change request.</param>
        /// <returns>
        /// An <see cref="ApiResponseDto{T}"/> indicating whether the password was changed successfully.
        /// </returns>
        /// <response code="200">If the password was changed successfully.</response>
        /// <response code="400">If the request is invalid or the current password is incorrect.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ApiResponseDto<bool>>> ChangePassword(ChangePasswordRequestDto requestDto)
        {
            return await profileService.ChangePasswordAsync(this.User.GetUserId(), requestDto);
        }
    }
}