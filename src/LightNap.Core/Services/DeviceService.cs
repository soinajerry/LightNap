using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Profile.Dto.Response;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Services
{
    /// <summary>
    /// Service for managing devices.
    /// </summary>
    public class DeviceService(ApplicationDbContext db) : IDeviceService
    {
        /// <summary>
        /// Retrieves the list of devices for the specified user.
        /// </summary>
        /// <param name="requestingUserId">The ID of the user requesting the devices.</param>
        /// <returns>A list of devices associated with the user.</returns>
        public async Task<ApiResponseDto<IList<DeviceDto>>> GetDevicesAsync(string requestingUserId)
        {
            var tokens = await db.RefreshTokens
                            .Where(token => token.UserId == requestingUserId && !token.IsRevoked && token.Expires > DateTime.UtcNow)
                            .OrderByDescending(device => device.Expires)
                            .ToListAsync();

            return ApiResponseDto<IList<DeviceDto>>.CreateSuccess(tokens.ToDtoList());
        }

        /// <summary>
        /// Revokes a device for the specified user.
        /// </summary>
        /// <param name="requestingUserId">The ID of the user requesting the revocation.</param>
        /// <param name="deviceId">The ID of the device to be revoked.</param>
        /// <returns>A boolean indicating whether the revocation was successful.</returns>
        public async Task<ApiResponseDto<bool>> RevokeDeviceAsync(string requestingUserId, string deviceId)
        {
            var token = await db.RefreshTokens.FindAsync(deviceId);

            if (token?.UserId == requestingUserId)
            {
                token.IsRevoked = true;
                await db.SaveChangesAsync();
                return ApiResponseDto<bool>.CreateSuccess(true);
            }

            return ApiResponseDto<bool>.CreateError("Device not found.");
        }
    }
}
