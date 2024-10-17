using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    public class NewPasswordRequestDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
