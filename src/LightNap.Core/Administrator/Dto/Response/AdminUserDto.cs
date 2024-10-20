namespace LightNap.Core.Administrator.Dto.Response
{
    /// <summary>
    /// Represents a complete user DTO intended for use by Administrator accounts.
    /// </summary>
    public class AdminUserDto
    {
        /// <summary>
        /// Gets or sets the ID of the administrator user.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// Gets or sets the email of the administrator user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the administrator user.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the last modified date of the administrator user.
        /// </summary>
        public long LastModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the created date of the administrator user.
        /// </summary>
        public long CreatedDate { get; set; }
    }
}