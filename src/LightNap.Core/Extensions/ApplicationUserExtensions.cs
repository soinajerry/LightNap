using LightNap.Core.Administrator.Dto.Request;
using LightNap.Core.Administrator.Dto.Response;
using LightNap.Core.Identity;
using LightNap.Core.Profile.Dto.Request;
using LightNap.Core.Profile.Dto.Response;

namespace LightNap.Core.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static ProfileDto ToLoggedInUserDto(this ApplicationUser user)
        {
            return new ProfileDto()
            {
                Email = user.Email!,
                Id = user.Id,
                UserName = user.UserName!
            };
        }

        public static void UpdateLoggedInUser(this ApplicationUser user, UpdateProfileDto dto)
        {
            user.LastModifiedDate = DateTime.UtcNow;

            // Update other fields from the DTO.
        }

        public static AdminUserDto ToAdminUserDto(this ApplicationUser user)
        {
            return new AdminUserDto()
            {
                CreatedDate = new DateTimeOffset(user.CreatedDate).ToUnixTimeMilliseconds(),
                LastModifiedDate = new DateTimeOffset(user.LastModifiedDate).ToUnixTimeMilliseconds(),
                Email = user.Email!,
                Id = user.Id,
                UserName = user.UserName!
            };
        }

        public static List<AdminUserDto> ToAdminUserDtoList(this IEnumerable<ApplicationUser> users)
        {
            return users.Select(user => user.ToAdminUserDto()).ToList();
        }

        public static void UpdateAdminUserDto(this ApplicationUser user, UpdateAdminUserDto dto)
        {
            user.LastModifiedDate = DateTime.UtcNow;

            // Update other fields from the DTO.
        }

    }
}