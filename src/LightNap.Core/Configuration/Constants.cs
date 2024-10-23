namespace LightNap.Core.Configuration
{
    /// <summary>
    /// Contains constant values used in the configuration of the core library.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Contains constant values related to DTO lengths.
        /// </summary>
        internal static class Dto
        {
            public const int Max2FaCodeLength = 512;
            public const int MaxEmailLength = 256;
            public const int MaxPasswordLength = 128;
            public const int MaxDeviceDetailsLength = 512;
            public const int MaxPasswordResetTokenLength = 512;
            public const int MaxUserNameLength = 32;
        }
    }
}
