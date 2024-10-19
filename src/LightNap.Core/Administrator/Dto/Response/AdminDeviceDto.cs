namespace LightNap.Core.Administrator.Dto.Response
{
    public class AdminDeviceDto
    {
        public required string Id { get; set; }
        public long LastSeen { get; set; }
        public required string IpAddress { get; set; }
        public required string Details { get; set; }
        public long Expires { get; set; }
        public required string UserId { get; set; }
    }
}