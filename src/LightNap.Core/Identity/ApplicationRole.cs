using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace LightNap.Core.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public required string Description { get; set; }
        public required string DisplayName { get; set; }

        [SetsRequiredMembers]
        public ApplicationRole(string name, string displayName, string description) : base(name)
        {
            this.Description = description;
            this.DisplayName = displayName;
        }
    }
}
