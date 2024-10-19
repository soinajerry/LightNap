namespace LightNap.Core.Profile.Dto.Response
{
    public class DeviceDto
    {
        public required string Id { get; set; }
        public long LastSeen { get; set; }
        public required string IpAddress { get; set; }
        public required string Details { get; set; }
    }
}