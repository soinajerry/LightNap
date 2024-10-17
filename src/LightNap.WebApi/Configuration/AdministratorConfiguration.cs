namespace LightNap.WebApi.Configuration
{
    public class AdministratorConfiguration
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public string? Password { get; set; }
    }
}
