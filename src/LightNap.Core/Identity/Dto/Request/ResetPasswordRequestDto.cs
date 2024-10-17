using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    public class ResetPasswordRequestDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
    }
}
