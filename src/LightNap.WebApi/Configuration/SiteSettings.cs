namespace LightNap.WebApi.Configuration
{
    public class SiteSettings
    {
        public bool RequireTwoFactorForNewUsers { get; set; }
        public required string SiteUrlRootForEmails { get; set; }
    }
}
