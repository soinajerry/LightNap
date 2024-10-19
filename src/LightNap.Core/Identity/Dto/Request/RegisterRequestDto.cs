using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    public class RegisterRequestDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string UserName { get; set; }

        public bool RememberMe { get; set; }
        public required string DeviceDetails { get; set; }
    }
}
