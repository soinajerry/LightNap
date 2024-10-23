using LightNap.Core.Api;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;

/// <summary>
/// Service for managing the logged-in user's profile.
/// </summary>
public interface IProfileService
{
    /// <summary>
    /// Retrieves the profile of the requesting user.
    /// </summary>
    /// <param name="requestingUserId">The unique identifier of the requesting user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{ProfileDto}"/> with the user's profile.</returns>
    Task<ApiResponseDto<ProfileDto>> GetProfile(string requestingUserId);

    /// <summary>
    /// Updates the profile of the requesting user.
    /// </summary>
    /// <param name="requestingUserId">The unique identifier of the requesting user.</param>
    /// <param name="requestDto">The data transfer object containing the profile update information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{ProfileDto}"/> with the updated profile.</returns>
    Task<ApiResponseDto<ProfileDto>> UpdateProfileAsync(string requestingUserId, UpdateProfileDto requestDto);

    /// <summary>
    /// Changes the password of the requesting user.
    /// </summary>
    /// <param name="requestingUserId">The unique identifier of the requesting user.</param>
    /// <param name="requestDto">The data transfer object containing the password change information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{bool}"/> indicating whether the password change was successful.</returns>
    Task<ApiResponseDto<bool>> ChangePasswordAsync(string requestingUserId, ChangePasswordRequestDto requestDto);
}
