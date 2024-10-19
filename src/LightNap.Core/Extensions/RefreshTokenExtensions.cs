using LightNap.Core.Identity;
using LightNap.Core.Profile.Dto.Response;

namespace LightNap.Core.Extensions
{
    public static class RefreshTokenExtensions
    {
        public static List<DeviceDto> ToDtoList(this IEnumerable<RefreshToken> tokens)
        {
            return tokens.Select(token =>
                new DeviceDto()
                {
                    Details = token.Details,
                    Id = token.Id,
                    LastSeen = new DateTimeOffset(token.LastSeen).ToUnixTimeMilliseconds(),
                    IpAddress = token.IpAddress,
                }).ToList();
        }
    }
}