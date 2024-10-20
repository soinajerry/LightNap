using LightNap.Core.Configuration;
using System.ComponentModel.DataAnnotations;

namespace LightNap.Core.Identity.Dto.Request
{
    /// <summary>
    /// Represents the login request data transfer object.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [EmailAddress]
        [Required]
        [StringLength(Constants.Dto.MaxEmailLength)]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxPasswordLength)]
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remember the user.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets the device details.
        /// </summary>
        [Required]
        [StringLength(Constants.Dto.MaxDeviceDetailsLength)]
        public required string DeviceDetails { get; set; }
    }
}
