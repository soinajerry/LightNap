using LightNap.Core.Api;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;

namespace LightNap.Core.Profile.Interfaces
{
    /// <summary>
    /// Service for managing the logged-in user's profile.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Retrieves the profile of the requesting user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{ProfileDto}"/> with the user's profile.</returns>
        Task<ApiResponseDto<ProfileDto>> GetProfile();

        /// <summary>
        /// Updates the profile of the requesting user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the profile update information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{ProfileDto}"/> with the updated profile.</returns>
        Task<ApiResponseDto<ProfileDto>> UpdateProfileAsync(UpdateProfileDto requestDto);

        /// <summary>
        /// Changes the password of the requesting user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the password change information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{bool}"/> indicating whether the password change was successful.</returns>
        Task<ApiResponseDto<bool>> ChangePasswordAsync(ChangePasswordRequestDto requestDto);

        /// <summary>
        /// Retrieves the settings of the requesting user.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{SettingsDto}"/> with the user's settings.</returns>
        Task<ApiResponseDto<BrowserSettingsDto>> GetSettingsAsync();

        /// <summary>
        /// Updates the settings of the requesting user.
        /// </summary>
        /// <param name="requestDto">The data transfer object containing the settings update information.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{SettingsDto}"/> with the updated settings.</returns>
        Task<ApiResponseDto<bool>> UpdateSettingsAsync(BrowserSettingsDto requestDto);

        /// <summary>  
        /// Retrieves a list of devices for the specified user.  
        /// </summary>  
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{T}"/> with a list of <see cref="DeviceDto"/>.</returns>  
        Task<ApiResponseDto<IList<DeviceDto>>> GetDevicesAsync();

        /// <summary>  
        /// Revokes a device for the specified user.  
        /// </summary>  
        /// <param name="deviceId">The ID of the device to be revoked.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="ApiResponseDto{T}"/> with a boolean indicating success or failure.</returns>  
        Task<ApiResponseDto<bool>> RevokeDeviceAsync(string deviceId);
    }
}