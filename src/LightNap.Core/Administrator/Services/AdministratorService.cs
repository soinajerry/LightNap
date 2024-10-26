using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Administrator.Interfaces;
using LightNap.Core.Administrator.Models;
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Identity.Dto.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Administrator.Services
{
    /// <summary>
    /// Service for managing administrator-related operations.
    /// </summary>
    public class AdministratorService(UserManager<ApplicationUser> userManager, ApplicationDbContext db) : IAdministratorService
    {
        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve.</param>
        /// <returns>The user details.</returns>
        public async Task<ApiResponseDto<AdminUserDto?>> GetUserAsync(string userId)
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
        public async Task<ApiResponseDto<PagedResponse<AdminUserDto>>> SearchUsersAsync(SearchUsersRequestDto requestDto)
        {
            IQueryable<ApplicationUser> query = db.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(requestDto.Email))
            {
                query = query.Where(user => EF.Functions.Like(user.Email!.ToLower(), $"%{requestDto.Email.ToLower()}%"));
            }

            if (!string.IsNullOrWhiteSpace(requestDto.UserName))
            {
                query = query.Where(user => EF.Functions.Like(user.UserName!.ToLower(), $"%{requestDto.UserName.ToLower()}%"));
            }

            switch (requestDto.SortBy)
            {
                case ApplicationUserSortBy.Email:
                    query = requestDto.ReverseSort ? query.OrderByDescending(user => user.Email) : query.OrderBy(user => user.Email);
                    break;
                case ApplicationUserSortBy.UserName:
                    query = requestDto.ReverseSort ? query.OrderByDescending(user => user.UserName) : query.OrderBy(user => user.UserName);
                    break;
                case ApplicationUserSortBy.CreatedDate:
                    query = requestDto.ReverseSort ? query.OrderByDescending(user => user.CreatedDate) : query.OrderBy(user => user.CreatedDate);
                    break;
                case ApplicationUserSortBy.LastModifiedDate:
                    query = requestDto.ReverseSort ? query.OrderByDescending(user => user.LastModifiedDate) : query.OrderBy(user => user.LastModifiedDate);
                    break;
                default: throw new ArgumentException($"Invalid sort field: '{requestDto.SortBy}'", nameof(requestDto.SortBy));
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
        /// <param name="userId">The ID of the user to update.</param>
        /// <param name="requestDto">The updated user information.</param>
        /// <returns>The updated user details.</returns>
        public async Task<ApiResponseDto<AdminUserDto>> UpdateUserAsync(string userId, UpdateAdminUserDto requestDto)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { return ApiResponseDto<AdminUserDto>.CreateError("The specified user was not found."); }

            user.UpdateAdminUserDto(requestDto);

            await db.SaveChangesAsync();

            return ApiResponseDto<AdminUserDto>.CreateSuccess(user.ToAdminUserDto());
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="userId">The ID of the user to delete.</param>
        /// <returns>True if the user was successfully deleted.</returns>
        public async Task<ApiResponseDto<bool>> DeleteUserAsync(string userId)
        {
            var user = await db.Users.FindAsync(userId);
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
        public ApiResponseDto<IList<RoleDto>> GetRoles()
        {
            return ApiResponseDto<IList<RoleDto>>.CreateSuccess(ApplicationRoles.All.ToDtoList());
        }

        /// <summary>
        /// Retrieves the roles for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The list of roles for the user.</returns>
        public async Task<ApiResponseDto<IList<string>>> GetRolesForUserAsync(string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { return ApiResponseDto<IList<string>>.CreateError("The specified user was not found."); }

            var roles = await userManager.GetRolesAsync(user);

            return ApiResponseDto<IList<string>>.CreateSuccess(roles);
        }

        /// <summary>
        /// Retrieves the users in a specific role.
        /// </summary>
        /// <param name="role">The role to search for.</param>
        /// <returns>The list of users in the specified role.</returns>
        public async Task<ApiResponseDto<IList<AdminUserDto>>> GetUsersInRoleAsync(string role)
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
        public async Task<ApiResponseDto<bool>> AddUserToRoleAsync(string role, string userId)
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
        public async Task<ApiResponseDto<bool>> RemoveUserFromRoleAsync(string role, string userId)
        {
            var user = await db.Users.FindAsync(userId);
            if (user is null) { return ApiResponseDto<bool>.CreateError("The specified user was not found."); }

            var result = await userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                return ApiResponseDto<bool>.CreateError(result.Errors.Select(error => error.Description));
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }
    }
}
