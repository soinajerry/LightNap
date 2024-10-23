using LightNap.Core.Api;
using LightNap.Core.Identity;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Core.Administrator.Dto.Response
{
    public class AdminAppConfigurationDto
    {
        public required IEnumerable<OptionDto> UserSortOptions { get; set; }

        [SetsRequiredMembers]
        public AdminAppConfigurationDto()
        {
            this.UserSortOptions = ApplicationUserSortOptions.All;
        }
    }
}
