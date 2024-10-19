using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.WebApi.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Authorize(Policy = Policies.RequireAdministratorRole)]
    [Route("api/[controller]")]
    public class AdministratorController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AdministratorController(ILogger<ProfileController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this._logger = logger;
            this._userManager = userManager;
            this._db = db;
        }

        [HttpGet("users/{id}")]
        public async Task<ActionResult<ApiResponseDto<AdminUserDto?>>> GetUser(string id)
        {
            var user = await this._db.Users.FindAsync(id);
            return ApiResponseDto<AdminUserDto?>.CreateSuccess(user?.ToAdminUserDto());
        }

        [HttpPost("users/search")]
        public async Task<ActionResult<ApiResponseDto<IList<AdminUserDto>>>> SearchUsers(SearchUsersRequestDto requestDto)
        {
            IQueryable<ApplicationUser> query = this._db.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(requestDto.Email))
            {
                query = query.Where(user => user.Email == requestDto.Email);
            }

            if (!string.IsNullOrWhiteSpace(requestDto.UserName))
            {
                query = query.Where(user => user.UserName == requestDto.UserName);
            }

            if (requestDto.PageNumber > 1)
            {
                query = query.Skip(requestDto.PageNumber * requestDto.PageSize);
            }

            var users = await query.Take(requestDto.PageSize).Select(user => user.ToAdminUserDto()).ToListAsync();

            return ApiResponseDto<IList<AdminUserDto>>.CreateSuccess(users);
        }

        [HttpPut("users/{id}")]
        public async Task<ActionResult<ApiResponseDto<AdminUserDto>>> UpdateUser(string id, UpdateAdminUserDto requestDto)
        {
            var user = await this._db.Users.FindAsync(id);
            if (user is null) { return ApiResponseDto<AdminUserDto>.CreateError("The specified user was not found."); }

            user.UpdateAdminUserDto(requestDto);

            await this._db.SaveChangesAsync();

            return ApiResponseDto<AdminUserDto>.CreateSuccess(user.ToAdminUserDto());
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteUser(string id)
        {
            var user = await this._db.Users.FindAsync(id);
            if (user is null) { return ApiResponseDto<bool>.CreateError("The specified user was not found."); }

            if (await this._userManager.IsInRoleAsync(user, ApplicationRoles.Administrator.Name!)) { return ApiResponseDto<bool>.CreateError("You may not delete an Administrator."); }

            this._db.Users.Remove(user);

            await this._db.SaveChangesAsync();

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        [HttpGet("users/{id}/roles")]
        public async Task<ActionResult<ApiResponseDto<IList<string>>>> GetRolesForUser(string id)
        {
            var user = await this._db.Users.FindAsync(id);
            if (user is null) { return ApiResponseDto<IList<string>>.CreateError("The specified user was not found."); }

            var roles = await this._userManager.GetRolesAsync(user);

            return ApiResponseDto<IList<string>>.CreateSuccess(roles);
        }

        [HttpGet("roles/{role}")]
        public async Task<ActionResult<ApiResponseDto<IList<AdminUserDto>>>> GetUsersInRole(string role)
        {
            var users = await this._userManager.GetUsersInRoleAsync(role);
            return ApiResponseDto<IList<AdminUserDto>>.CreateSuccess(users.ToAdminUserDtoList());
        }

        [HttpPost("roles/{role}/{userId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> AddUserToRole(string role, string userId)
        {
            var user = await this._db.Users.FindAsync(userId);
            if (user is null) { return ApiResponseDto<bool>.CreateError("The specified user was not found."); }

            var result = await this._userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return ApiResponseDto<bool>.CreateError(result.Errors.Select(error => error.Description));
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        [HttpDelete("roles/{role}/{userId}")]
        public async Task<ActionResult<ApiResponseDto<bool>>> RemoveUserFromRole(string role, string userId)
        {
            var user = await this._db.Users.FindAsync(userId);
            if (user is null) { return ApiResponseDto<bool>.CreateError("The specified user was not found."); }

            var result = await this._userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return ApiResponseDto<bool>.CreateError(result.Errors.Select(error => error.Description));
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

    }
}