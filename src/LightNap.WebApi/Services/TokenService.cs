using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.WebApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LightNap.WebApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            this._configuration = configuration;
            this._userManager = userManager;
        }

        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration.GetRequiredSetting("Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            IList<string>? roles = await this._userManager.GetRolesAsync(user);
            if ((roles != null) && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var token = new JwtSecurityToken(
                issuer: this._configuration.GetRequiredSetting("Jwt:Issuer"),
                audience: this._configuration.GetRequiredSetting("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(this._configuration.GetRequiredSetting("Jwt:ExpirationMinutes"))),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }

}
