namespace LightNap.Core.Identity
{
    public static class ApplicationRoles
    {
        public static readonly ApplicationRole Administrator = new("Administrator", "Administrator", "Access to all administrative features");
        public static IReadOnlyList<ApplicationRole> All =>
        [
            ApplicationRoles.Administrator
        ];
    }
}
