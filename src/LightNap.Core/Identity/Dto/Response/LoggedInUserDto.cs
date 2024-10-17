namespace LightNap.Core.Identity.Dto.Response
{
    public class LoggedInUserDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
    }
}