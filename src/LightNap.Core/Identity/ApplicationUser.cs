using Microsoft.AspNetCore.Identity;

namespace LightNap.Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
