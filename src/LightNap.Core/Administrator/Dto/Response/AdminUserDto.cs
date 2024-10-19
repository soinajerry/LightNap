namespace LightNap.Core.Administrator.Dto.Response
{
    public class AdminUserDto
    {
        public required string Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public long LastModifiedDate { get; set; }
        public long CreatedDate { get; set; }
    }
}