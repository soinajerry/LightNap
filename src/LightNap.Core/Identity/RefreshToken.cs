namespace LightNap.Core.Identity
{
    public class RefreshToken
    {
        public required string Id { get; set; }
        public required string Token { get; set; }
        public DateTime LastSeen { get; set; }
        public required string IpAddress { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }
        public required string Details { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }

}
