using LightNap.Core.Profile.Dto.Response;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Core.Data.Entities
{
    /// <summary>
    /// Represents an application user with additional properties.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The date when the user was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The date when the user was last modified.
        /// </summary>
        public DateTime LastModifiedDate { get; set; }

        /// <summary>
        /// The user's browser app settings.
        /// </summary>
        public virtual BrowserSettingsDto BrowserSettings { get; set; }

        /// <summary>
        /// The refresh tokens associated with the user.
        /// </summary>
        public ICollection<RefreshToken>? RefreshTokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUser"/> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="twoFactorEnabled">A value indicating whether two-factor authentication is enabled.</param>
        [SetsRequiredMembers]
        public ApplicationUser(string userName, string email, bool twoFactorEnabled)
        {
            BrowserSettings = new BrowserSettingsDto();
            CreatedDate = DateTime.UtcNow;
            Email = email;
            LastModifiedDate = DateTime.UtcNow;
            TwoFactorEnabled = twoFactorEnabled;
            UserName = userName;
        }
    }
}
