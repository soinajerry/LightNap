namespace LightNap.Core.Identity.Dto.Response
{
    public class AdminDeviceDto
    {
        public required string Id { get; set; }
        public long LastSeen { get; set; }
        public required string IpAddress { get; set; }
        public required string DeviceId { get; set; }
        public long Expires { get; set; }
        public required string UserId { get; set; }
    }
}