using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public required string CurrentPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
        [Required]
        public required string ConfirmNewPassword { get; set; }
    }
}
