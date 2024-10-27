using LightNap.Core.Interfaces;

namespace LightNap.Core.Tests
{
    internal class TestUserContext : IUserContext
    {
        public string? IpAddress { get; set; }
        public string? UserId { get; set; }

        public string? GetIpAddress()
        {
            return this.IpAddress;
        }

        public string GetUserId()
        {
            if (this.UserId is null) { throw new InvalidOperationException("GetUserId was called without having UserId set first");  }
            return this.UserId;
        }
    }
}
