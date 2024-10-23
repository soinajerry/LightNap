using LightNap.Core.Api;

namespace LightNap.Core.Administrator.Dto.Request
{
    /// <summary>
    /// Represents a request to search users.
    /// </summary>
    public class SearchUsersRequestDto : PaginationRequestBase
    {
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the field to sort the users by.
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// Reverses the default sort behavior of the SortBy field.
        /// </summary>
        public bool ReverseSort { get; set; }
    }
}