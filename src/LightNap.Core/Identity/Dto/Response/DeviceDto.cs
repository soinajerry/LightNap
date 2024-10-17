namespace LightNap.Core.Identity.Dto.Response
{
    public class DeviceDto
    {
        public required string Id { get; set; }
        public long LastSeen { get; set; }
        public required string IpAddress { get; set; }
        public required string DeviceId { get; set; }
    }
}