using LightNap.Core.Api;
using LightNap.Core.Profile.Dto.Response;

public interface IDeviceService
{
    Task<ApiResponseDto<IList<DeviceDto>>> GetDevicesAsync(string userId);
    Task<ApiResponseDto<bool>> RevokeDeviceAsync(string requestingUserId, string deviceId);
}