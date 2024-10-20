namespace LightNap.WebApi.Configuration
{
    /// <summary>
    /// Contains constant values used in the configuration of the web API.
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// Contains constant values related to cookies.
        /// </summary>
        internal class Cookies
        {
            /// <summary>
            /// The name of the refresh token cookie.
            /// </summary>
            public const string RefreshToken = "refreshToken";

            /// <summary>
            /// The "Max-Age" string used in cookies.
            /// </summary>
            public const string MaxAge = "Max-Age";

            /// <summary>
            /// The "Expires" string used in cookies.
            /// </summary>
            public const string Expires = "Expires";
        }

        /// <summary>
        /// Contains constant values related to refresh tokens.
        /// </summary>
        internal class RefreshTokens
        {
            public const string NoIpProvided = "No IP Provided";
        }
    }
}
