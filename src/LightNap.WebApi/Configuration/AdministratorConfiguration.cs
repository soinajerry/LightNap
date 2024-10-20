namespace LightNap.WebApi.Configuration
{
    /// <summary>
    /// Represents the configuration for an administrator.
    /// </summary>
    public class AdministratorConfiguration
    {
        /// <summary>
        /// Gets or sets the email of the administrator. This field is required.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the administrator. This field is required.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password of the administrator. This field is optional.
        /// </summary>
        public string? Password { get; set; }
    }
}
