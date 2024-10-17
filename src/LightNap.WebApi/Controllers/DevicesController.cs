using LightNap.Core;
using LightNap.Core.Api;
using LightNap.Core.Extensions;
using LightNap.Core.Identity.Dto.Response;
using LightNap.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _db;

        public DevicesController(ILogger<ProfileController> logger, ApplicationDbContext db)
        {
            this._logger = logger;
            this._db = db;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IList<DeviceDto>>>> GetDevices()
        {
            var tokens = await this._db.RefreshTokens.Where(token => token.UserId == this.User.GetUserId() && !token.IsRevoked && token.Expires > DateTime.UtcNow).ToListAsync();

            return ApiResponseDto<IList<DeviceDto>>.CreateSuccess(tokens.ToDtoList());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> RevokeDevice(string id)
        {
            var token = await this._db.RefreshTokens.FindAsync(id);

            if (token?.UserId == this.User.GetUserId())
            {
                token.IsRevoked = true;
                await this._db.SaveChangesAsync();
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

    }
}