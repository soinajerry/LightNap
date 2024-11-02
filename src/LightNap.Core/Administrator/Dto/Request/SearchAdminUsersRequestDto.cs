using LightNap.Core.Administrator.Models;
using LightNap.Core.Api;
using LightNap.Core.Identity;

namespace LightNap.Core.Administrator.Dto.Request
{
    /// <summary>
    /// Represents a request to search users.
    /// </summary>
    public class SearchAdminUsersRequestDto : PaginationRequestDtoBase
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
        public ApplicationUserSortBy SortBy { get; set; }

        /// <summary>
        /// Reverses the default sort behavior of the SortBy field.
        /// </summary>
        public bool ReverseSort { get; set; }
    }
}