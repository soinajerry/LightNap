using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Interfaces;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;
using Microsoft.AspNetCore.Identity;

namespace LightNap.Core.Services.Application
{
    /// <summary>
    /// Service for managing user profiles.
    /// </summary>
    public class ProfileService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IUserContext userContext) : IProfileService
    {
        /// <summary>
        /// Changes the password for the specified user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the current and new passwords.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> indicating the success or failure of the operation.</returns>
        public async Task<ApiResponseDto<bool>> ChangePasswordAsync(ChangePasswordRequestDto requestDto)
        {
            if (requestDto.NewPassword != requestDto.ConfirmNewPassword) { return ApiResponseDto<bool>.CreateError("New password does not match confirmation password."); }

            ApplicationUser? user = await userManager.FindByIdAsync(userContext.GetUserId());
            if (user is null) { return ApiResponseDto<bool>.CreateError("Unable to change password."); }

            var result = await userManager.ChangePasswordAsync(user, requestDto.CurrentPassword, requestDto.NewPassword);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { return ApiResponseDto<bool>.CreateError(result.Errors.Select(item => item.Description).ToArray()); }
                return ApiResponseDto<bool>.CreateError("Unable to change password.");
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// Retrieves the profile of the specified user.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the user's profile.</returns>
        public async Task<ApiResponseDto<ProfileDto>> GetProfile()
        {
            var user = await db.Users.FindAsync(userContext.GetUserId());
            return ApiResponseDto<ProfileDto>.CreateSuccess(user?.ToLoggedInUserDto());
        }

        /// <summary>
        /// Updates the profile of the specified user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the updated profile information.</param>
        /// <returns>An <see cref="ApiResponseDto{T}"/> indicating the success or failure of the operation.</returns>
        public async Task<ApiResponseDto<ProfileDto>> UpdateProfileAsync(UpdateProfileDto requestDto)
        {
            var user = await db.Users.FindAsync(userContext.GetUserId());
            if (user is null) { return ApiResponseDto<ProfileDto>.CreateError("Unable to change password."); }

            user.UpdateLoggedInUser(requestDto);

            await db.SaveChangesAsync();

            return ApiResponseDto<ProfileDto>.CreateSuccess(user.ToLoggedInUserDto());
        }
    }
}
