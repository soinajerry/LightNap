using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Api;
using LightNap.Core.Identity.Dto.Response;

public interface IAdministratorService
{
    Task<ApiResponseDto<AdminUserDto?>> GetUserAsync(string userId);
    Task<ApiResponseDto<PagedResponse<AdminUserDto>>> SearchUsersAsync(SearchUsersRequestDto requestDto);
    Task<ApiResponseDto<AdminUserDto>> UpdateUserAsync(string id, UpdateAdminUserDto requestDto);
    Task<ApiResponseDto<bool>> DeleteUserAsync(string id);
    ApiResponseDto<IList<RoleDto>> GetRoles();
    Task<ApiResponseDto<IList<string>>> GetRolesForUserAsync(string id);
    Task<ApiResponseDto<IList<AdminUserDto>>> GetUsersInRoleAsync(string role);
    Task<ApiResponseDto<bool>> AddUserToRoleAsync(string role, string userId);
    Task<ApiResponseDto<bool>> RemoveUserFromRoleAsync(string role, string userId);
}