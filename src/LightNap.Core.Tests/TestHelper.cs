using LightNap.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LightNap.Core.Tests
{
    internal static class TestHelper
    {
        public static async Task<ApplicationUser> CreateTestUserAsync(UserManager<ApplicationUser> userManager, string userId, string? userName = null, string? email = null)
        {
            userName ??= Guid.NewGuid().ToString("N");
            email ??= $"{Guid.NewGuid():N}@test.com";
            ApplicationUser user = new(userName, email, false) { Id = userId };
            await userManager.CreateAsync(user);
            return user;
        }

        public static async Task<ApplicationRole> CreateTestRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            ApplicationRole role = new(roleName, roleName, roleName);
            await roleManager.CreateAsync(role);
            return role;
        }
    }
}
