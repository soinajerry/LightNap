using Microsoft.AspNetCore.Identity;

namespace LightNap.Core.Identity
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
        /// The refresh tokens associated with the user.
        /// </summary>
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
