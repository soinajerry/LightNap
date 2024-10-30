namespace LightNap.Core.Administrator.Dto.Response
{
    /// <summary>
    /// Represents a complete user DTO intended for use by Administrator accounts.
    /// </summary>
    public class AdminUserDto
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The email of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// The last modified date of the user.
        /// </summary>
        public long LastModifiedDate { get; set; }

        /// <summary>
        /// The created date of the user.
        /// </summary>
        public long CreatedDate { get; set; }

        /// <summary>
        /// When the user's lockout ends, if applicable.
        /// </summary>
        public long? LockoutEnd { get; set; }
    }
}