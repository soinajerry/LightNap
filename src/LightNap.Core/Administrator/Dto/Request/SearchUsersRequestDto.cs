using LightNap.Core.Api;

namespace LightNap.Core.Administrator.Dto.Request
{
    public class SearchUsersRequestDto : PaginationRequestBase
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? SortBy { get; set; }
    }
}