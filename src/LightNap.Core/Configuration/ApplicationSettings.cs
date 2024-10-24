namespace LightNap.Core.Configuration
{
    /// <summary>
    /// Represents the site settings for the web API.
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to automatically apply Entity Framework migrations.
        /// </summary>
        public bool AutomaticallyApplyEfMigrations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to require two-factor authentication for new users.
        /// </summary>
        public bool RequireTwoFactorForNewUsers { get; set; }

        /// <summary>
        /// Gets or sets the root URL for emails sent by the site.
        /// </summary>
        public required string SiteUrlRootForEmails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use SameSite strict cookies.
        /// </summary>
        public bool UseSameSiteStrictCookies { get; set; }
    }
}
