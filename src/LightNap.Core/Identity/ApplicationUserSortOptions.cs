using LightNap.Core.Api;

namespace LightNap.Core.Identity
{
    public static class ApplicationUserSortOptions
    {
        public static readonly OptionDto Email = new("email", "Email", "Sorts by email.");
        public static readonly OptionDto UserName = new("user-name", "UserName", "Sorts by UserName.");
        public static readonly OptionDto CreatedDate = new("created-date", "Created", "Sorts by when the user was created.");
        public static readonly OptionDto LastModifiedDate = new("last-modified-date", "Last Modified", "Sorts by when the user was last modified.");

        public static IReadOnlyList<OptionDto> All =>
        [
            ApplicationUserSortOptions.UserName,
            ApplicationUserSortOptions.CreatedDate,
            ApplicationUserSortOptions.LastModifiedDate,
            ApplicationUserSortOptions.Email,
        ];

    }
}
