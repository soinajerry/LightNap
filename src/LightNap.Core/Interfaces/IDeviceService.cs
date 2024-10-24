using LightNap.Core.Api;
using LightNap.Core.Profile.Dto.Response;

namespace LightNap.Core.Interfaces
{
    /// <summary>  
    /// Provides methods to manage devices.  
    /// </summary>  
    public interface IDeviceService
    {
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