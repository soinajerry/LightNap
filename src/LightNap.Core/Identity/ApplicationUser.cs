using Microsoft.AspNetCore.Identity;

namespace LightNap.Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
