using LightNap.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data;

namespace LightNap.WebApi.Configuration
{
    /// <summary>
    /// Class responsible for seeding roles and administrators.
    /// </summary>
    public static class Seeder
    {
        /// <summary>
        /// Seeds the roles in the application.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedRoles(RoleManager<ApplicationRole> roleManager, ILogger logger)
        {
            foreach (ApplicationRole role in ApplicationRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    await roleManager.CreateAsync(ApplicationRoles.Administrator);
                    logger.LogInformation("Added role '{roleName}'", role.Name);
                }
            }

            var roleSet = new HashSet<string>(ApplicationRoles.All.Select(role => role.Name!), StringComparer.OrdinalIgnoreCase);

            foreach (var role in roleManager.Roles.Where(role => role.Name != null && !roleSet.Contains(role.Name)))
            {
                await roleManager.DeleteAsync(role);
                logger.LogInformation("Removed role '{roleName}'", role.Name);
            }
        }

        /// <summary>
        /// Seeds the administrators in the application.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="administratorConfigurations">The administrator configurations.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAdministrators(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<List<AdministratorConfiguration>> administratorConfigurations, ILogger logger)
        {
            if (administratorConfigurations.Value is null) { return; }

            foreach (var administrator in administratorConfigurations.Value)
            {
                ApplicationUser? user = await userManager.FindByEmailAsync(administrator.Email);

                if (user is null)
                {
                    user = new ApplicationUser()
                    {
                        Email = administrator.Email,
                        EmailConfirmed = true,
                        UserName = administrator.UserName,
                    };

                    bool passwordProvided = !string.IsNullOrWhiteSpace(administrator.Password);
                    string password = passwordProvided ? administrator.Password! : $"P@ssw0rd{Guid.NewGuid()}";
                    var identity = await userManager.CreateAsync(user, password);
                    if (identity.Succeeded)
                    {
                        logger.LogInformation("Created administrator user '{userName}' ('{email}'): {passwordText}", administrator.UserName, administrator.Email,
                            passwordProvided ? "Provided password was used" : "Reset password to log in");
                    }
                    else
                    {
                        logger.LogError("Unable to create Administrator user for '{userName}' ('{email}'): {errors}", administrator.UserName, administrator.Email, string.Join("; ", identity.Errors.Select(error => error.Description)));
                        continue;
                    }
                }

                if (!await userManager.IsInRoleAsync(user, ApplicationRoles.Administrator.Name!))
                {
                    await userManager.AddToRoleAsync(user, ApplicationRoles.Administrator.Name!);
                    logger.LogInformation("Added administrator role for '{userName}' ('{email}')", administrator.UserName, administrator.Email);
                }
            }
        }
    }
}
