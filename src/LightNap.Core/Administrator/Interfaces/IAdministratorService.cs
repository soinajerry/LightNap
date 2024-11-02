using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Api;
using LightNap.Core.Identity.Dto.Response;

namespace LightNap.Core.Administrator.Interfaces
{
    /// <summary>  
    /// Interface for administrator services.  
    /// </summary>  
    public interface IAdministratorService
    {
        /// <summary>  
        /// Gets a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response with the user data.</returns>  
        Task<ApiResponseDto<AdminUserDto?>> GetUserAsync(string userId);

        /// <summary>  
        /// Searches users asynchronously based on the specified request DTO.  
        /// </summary>  
        /// <param name="requestDto">The request DTO containing search parameters.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response with the paged user data.</returns>  
        Task<ApiResponseDto<PagedResponse<AdminUserDto>>> SearchUsersAsync(SearchAdminUsersRequestDto requestDto);

        /// <summary>  
        /// Updates a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <param name="requestDto">The request DTO containing update information.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response with the updated user data.</returns>  
        Task<ApiResponseDto<AdminUserDto>> UpdateUserAsync(string userId, UpdateAdminUserDto requestDto);

        /// <summary>  
        /// Deletes a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response indicating whether the deletion was successful.</returns>  
        Task<ApiResponseDto<bool>> DeleteUserAsync(string userId);

        /// <summary>  
        /// Gets all roles.  
        /// </summary>  
        /// <returns>The API response with the list of roles.</returns>  
        ApiResponseDto<IList<RoleDto>> GetRoles();

        /// <summary>  
        /// Gets roles for a user asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response with the list of roles for the user.</returns>  
        Task<ApiResponseDto<IList<string>>> GetRolesForUserAsync(string userId);

        /// <summary>  
        /// Gets users in a role asynchronously by role name.  
        /// </summary>  
        /// <param name="role">The role name.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response with the list of users in the role.</returns>  
        Task<ApiResponseDto<IList<AdminUserDto>>> GetUsersInRoleAsync(string role);

        /// <summary>  
        /// Adds a user to a role asynchronously.  
        /// </summary>  
        /// <param name="role">The role name.</param>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response indicating whether the addition was successful.</returns>  
        Task<ApiResponseDto<bool>> AddUserToRoleAsync(string role, string userId);

        /// <summary>  
        /// Removes a user from a role asynchronously.  
        /// </summary>  
        /// <param name="role">The role name.</param>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response indicating whether the removal was successful.</returns>  
        Task<ApiResponseDto<bool>> RemoveUserFromRoleAsync(string role, string userId);

        /// <summary>  
        /// Locks a user account asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response indicating whether the lock was successful.</returns>  
        Task<ApiResponseDto<bool>> LockUserAccountAsync(string userId);

        /// <summary>  
        /// Unlocks a user account asynchronously by user ID.  
        /// </summary>  
        /// <param name="userId">The user ID.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the API response indicating whether the unlock was successful.</returns>  
        Task<ApiResponseDto<bool>> UnlockUserAccountAsync(string userId);
    }
}