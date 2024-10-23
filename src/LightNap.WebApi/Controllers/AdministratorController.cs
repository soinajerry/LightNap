using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.Core.Identity.Dto.Response;
using LightNap.WebApi.Configuration;
using LightNap.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LightNap.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing administrative tasks.
    /// </summary>
    [ApiController]
    [Authorize(Policy = Policies.RequireAdministratorRole)]
    [Route("api/[controller]")]
    public class AdministratorController(UserManager<ApplicationUser> userManager, ApplicationDbContext db) : ControllerBase
    {
        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user details.</returns>
        /// <response code="200">Returns the user details.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("users/{userId}")]
        [ProducesResponseType(typeof(ApiResponseDto<AdminUserDto?>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponseDto<AdminUserDto?>>> GetUser(string userId)
        {
            var user = await db.Users.FindAsync(userId);
            // NOTE: The requested user not being found is a successful request if nothing fails. As a result, we return null as
            // confirmation that the request succeeded but there is no user matching that ID. It makes client development easier.
            return ApiResponseDto<AdminUserDto?>.CreateSuccess(user?.ToAdminUserDto());
        }

        /// <summary>
        /// Searches for users based on the specified criteria.
        /// </summary>
        /// <param name="requestDto">The search criteria.</param>
        /// <returns>The list of users matching the criteria.</returns>
        /// <response code="200">Returns the list of users.</response>
        [HttpPost("users/search")]
        [ProducesResponseType(typeof(ApiResponseDto<PagedResponse<AdminUserDto>>), 200)]
        public async Task<ActionResult<ApiResponseDto<PagedResponse<AdminUserDto>>>> SearchUsers(SearchUsersRequestDto requestDto)
        {
            IQueryable<ApplicationUser> query = db.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(requestDto.Email))
            {
                query = query.Where(user => user.Email == requestDto.Email);
            }

            if (!string.IsNullOrWhiteSpace(requestDto.UserName))
            {
                query = query.Where(user => user.UserName == requestDto.UserName);
            }

            string sortBy = requestDto.SortBy ?? ApplicationUserSortOptions.UserName.Key;
            if (sortBy == ApplicationUserSortOptions.Email.Key)
            {
                query = requestDto.ReverseSort ? query.OrderByDescending(user => user.Email) : query.OrderBy(user => user.Email);
            }
            else if (sortBy == ApplicationUserSortOptions.CreatedDate.Key)
            {
                query = requestDto.ReverseSort ? query.OrderByDescending(user => user.CreatedDate) : query.OrderBy(user => user.CreatedDate);
            }
            else if (sortBy == ApplicationUserSortOptions.LastModifiedDate.Key)
            {
                query = requestDto.ReverseSort ? query.OrderByDescending(user => user.LastModifiedDate) : query.OrderBy(user => user.LastModifiedDate);
            }
            else //if (sortBy == ApplicationUserSortOptions.UserName.Key)
            {
                query = requestDto.ReverseSort ? query.OrderByDescending(user => user.UserName) : query.OrderBy(user => user.UserName);
            }

            int totalCount = await query.CountAsync();

            if (requestDto.PageNumber > 1)
            {
                query = query.Skip((requestDto.PageNumber - 1) * requestDto.PageSize);
            }

            var users = await query.Take(requestDto.PageSize).Select(user => user.ToAdminUserDto()).ToListAsync();

            return ApiResponseDto<PagedResponse<AdminUserDto>>.CreateSuccess(
                new PagedResponse<AdminUserDto>(users, requestDto.PageNumber, requestDto.PageSize, totalCount));
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="requestDto">The updated user information.</param>
        /// <returns>The updated user details.</returns>
        /// <response code="200">Returns the updated user details.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpPut("users/{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<AdminUserDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponseDto<AdminUserDto>>> UpdateUser(string id, UpdateAdminUserDto requestDto)
        {
            var user = await db.Users.FindAsync(id);
            if (user is null) { return ApiResponseDto<AdminUserDto>.CreateError("The specified user was not found."); }

            user.UpdateAdminUserDto(requestDto);

            await db.SaveChangesAsync();

            return ApiResponseDto<AdminUserDto>.CreateSuccess(user.ToAdminUserDto());
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted.</returns>
        /// <response code="200">User successfully deleted.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="400">If the user is an administrator and cannot be deleted.</response>
        [HttpDelete("users/{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteUser(string id)
        {
            var user = await db.Users.FindAsync(id);
            if (user is null) { return ApiResponseDto<bool>.CreateError("The specified user was not found."); }

            if (await userManager.IsInRoleAsync(user, ApplicationRoles.Administrator.Name!)) { return ApiResponseDto<bool>.CreateError("You may not delete an Administrator."); }

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// Retrieves all available roles.
        /// </summary>
        /// <returns>The list of roles.</returns>
        /// <response code="200">Returns the list of roles.</response>
        [HttpGet("roles")]
        [ProducesResponseType(typeof(ApiResponseDto<IList<RoleDto>>), 200)]
        public ActionResult<ApiResponseDto<IList<RoleDto>>> GetRoles()
        {
            return ApiResponseDto<IList<RoleDto>>.CreateSuccess(ApplicationRoles.All.ToDtoList());
        }

        /// <summary>
        /// Retrieves the roles for a user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The list of roles for the user.</returns>
        /// <response code="200">Returns the list of roles.</response>
        /// <response code="404">If the user is not found.</response>
        [HttpGet("users/{id}/roles")]
        [ProducesResponseType(typeof(ApiResponseDto<IList<string>>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponseDto<IList<string>>>> GetRolesForUser(string id)
        {
            var user = await db.Users.FindAsync(id);
            if (user is null) { return ApiResponseDto<IList<string>>.CreateError("The specified user was not found."); }

            var roles = await userManager.GetRolesAsync(user);

            return ApiResponseDto<IList<string>>.CreateSuccess(roles);
        }

        /// <summary>
        /// Retrieves the users in a specific role.
        /// </summary>
        /// <param name="role">The role to search for.</param>
        /// <returns>The list of users in the specified role.</returns>
        /// <response code="200">Returns the list of users.</response>
        [HttpGet("roles/{role}")]
        [ProducesResponseType(typeof(ApiResponseDto<IList<AdminUserDto>>), 200)]
        public async Task<ActionResult<ApiResponseDto<IList<AdminUserDto>>>> GetUsersInRole(string role)
        {
            var users = await userManager.GetUsersInRoleAsync(role);
            return ApiResponseDto<IList<AdminUserDto>>.CreateSuccess(users.ToAdminUserDtoList());
        }

        /// <summary>
        /// Adds a user to a role.
        /// </summary>
        /// <param name="role">The role to add the user to.</param>
        /// <param name="userId">The ID of the user to add to the role.</param>
        /// <returns>True if the user was successfully added to the role.</returns>
        /// <response code="200">User successfully added to the role.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="400">If there was an error adding the user to the role.</response>
        [HttpPost("roles/{role}/{userId}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<bool>>> AddUserToRole(string role, string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { return ApiResponseDto<bool>.CreateError("The specified user was not found."); }

            var result = await userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return ApiResponseDto<bool>.CreateError(result.Errors.Select(error => error.Description));
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// Removes a user from a role.
        /// </summary>
        /// <param name="role">The role to remove the user from.</param>
        /// <param name="userId">The ID of the user to remove from the role.</param>
        /// <returns>True if the user was successfully removed from the role.</returns>
        /// <response code="200">User successfully removed from the role.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="400">If there was an error removing the user from the role.</response>
        [HttpDelete("roles/{role}/{userId}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<bool>>> RemoveUserFromRole(string role, string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { return ApiResponseDto<bool>.CreateError("The specified user was not found."); }

            if ((user.Id == this.User.GetUserId()) && (role == ApplicationRoles.Administrator.Name)) { return ApiResponseDto<bool>.CreateError("You may not remove yourself from the Administrator role."); }

            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return ApiResponseDto<bool>.CreateError(result.Errors.Select(error => error.Description));
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// Retrieves the application configuration for the admin app.
        /// </summary>
        /// <returns>The admin app configuration.</returns>
        /// <response code="200">Returns the admin app configuration.</response>
        [HttpGet("app-configuration")]
        [ProducesResponseType(typeof(ApiResponseDto<AdminAppConfigurationDto>), 200)]
        public ActionResult<ApiResponseDto<AdminAppConfigurationDto>> GetAdminAppConfiguration()
        {
            return ApiResponseDto<AdminAppConfigurationDto>.CreateSuccess(new AdminAppConfigurationDto());
        }
    }
}