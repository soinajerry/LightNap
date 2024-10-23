using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Profile.Dto.Response;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Services
{
    public class DeviceService(ApplicationDbContext db) : IDeviceService
    {
        public async Task<ApiResponseDto<IList<DeviceDto>>> GetDevicesAsync(string userId)
        {
            var tokens = await db.RefreshTokens
                            .Where(token => token.UserId == userId && !token.IsRevoked && token.Expires > DateTime.UtcNow)
                            .OrderByDescending(device => device.Expires)
                            .ToListAsync();

            return ApiResponseDto<IList<DeviceDto>>.CreateSuccess(tokens.ToDtoList());
        }

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
