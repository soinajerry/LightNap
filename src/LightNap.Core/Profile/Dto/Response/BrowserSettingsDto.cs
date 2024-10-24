namespace LightNap.Core.Profile.Dto.Response
{
    /// <summary>
    /// Data transfer object representing the user's settings for the browser app.
    /// </summary>
    public class BrowserSettingsDto
    {
        /// <summary>
        /// The version of this setting to support migrations if necessary.
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// The selected browser app theme.
        /// </summary>
        public string? Theme { get; set; } = string.Empty;

        /// <summary>
        /// Contains extended settings that might be less commonly set and/or temporary (like feature flags).
        /// </summary>
        public Dictionary<string, object> Extended { get; set; } = new Dictionary<string, object>();
    }
}