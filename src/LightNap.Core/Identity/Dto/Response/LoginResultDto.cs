namespace LightNap.Core.Identity.Dto.Response
{
    /// <summary>
    /// Represents the result of a login operation.
    /// </summary>
    public class LoginResultDto
    {
        /// <summary>
        /// Gets or sets a value indicating whether two-factor authentication is required.
        /// </summary>
        public bool TwoFactorRequired { get; set; }

        /// <summary>
        /// Gets or sets the bearer token for the authenticated user.
        /// </summary>
        public string? BearerToken { get; set; }
    }
}