using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Profile.Dto.Response;
using LightNap.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DevicesController(ApplicationDbContext db) : ControllerBase
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
            var tokens = await db.RefreshTokens
                .Where(token => token.UserId == this.User.GetUserId() && !token.IsRevoked && token.Expires > DateTime.UtcNow)
                .OrderByDescending(device => device.Expires)
                .ToListAsync();

            return ApiResponseDto<IList<DeviceDto>>.CreateSuccess(tokens.ToDtoList());
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
            var token = await db.RefreshTokens.FindAsync(deviceId);

            if (token?.UserId == this.User.GetUserId())
            {
                token.IsRevoked = true;
                await db.SaveChangesAsync();
                return ApiResponseDto<bool>.CreateSuccess(true);
            }

            return ApiResponseDto<bool>.CreateError("Device not found.");
        }

    }
}