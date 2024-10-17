using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    public class VerifyCodeRequestDto
    {
        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string Code { get; set; }

        public bool RememberMe { get; set; }
        public string? DeviceId { get; set; }
    }
}