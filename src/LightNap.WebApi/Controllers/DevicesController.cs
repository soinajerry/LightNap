using LightNap.Core.Api;
using LightNap.Core.Interfaces;
using LightNap.Core.Profile.Dto.Response;
using LightNap.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DevicesController(IDeviceService deviceService) : ControllerBase
    {
        /// <summary>
        /// Retrieves the list of devices.
        /// </summary>
        /// <returns>The list of devices.</returns>
        /// <response code="200">Returns the list of devices.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<IList<DeviceDto>>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ApiResponseDto<IList<DeviceDto>>>> GetDevices()
        {
            return await deviceService.GetDevicesAsync(this.User.GetUserId());
        }

        /// <summary>
        /// Revokes a device.
        /// </summary>
        /// <param name="deviceId">The ID of the device to revoke.</param>
        /// <returns>A response indicating whether the device was successfully revoked.</returns>
        /// <response code="200">Device successfully revoked.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Device not found.</response>
        [HttpDelete("{deviceId}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponseDto<bool>>> RevokeDevice(string deviceId)
        {
            return await deviceService.RevokeDeviceAsync(this.User.GetUserId(), deviceId);
        }

    }
}