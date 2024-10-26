using LightNap.Core.Data.Entities;
using LightNap.Core.Extensions;
using LightNap.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LightNap.Core.Services
{
    /// <summary>
    /// Service for generating JWT access tokens and refresh tokens.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SigningCredentials _signingCredentials;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        /// <param name="userManager">The user manager.</param>
        public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            var tokenKey = _configuration.GetRequiredSetting("Jwt:Key");
            if (tokenKey.Length < 32) { throw new ArgumentException("The provided setting 'Jwt:Key' must be at least 32 characters long"); }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            _issuer = _configuration.GetRequiredSetting("Jwt:Issuer");
            _audience = _configuration.GetRequiredSetting("Jwt:Audience");
            _expirationMinutes = int.Parse(_configuration.GetRequiredSetting("Jwt:ExpirationMinutes"));
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Generates an access token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>The generated access token.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user parameter is null.</exception>
        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new(JwtRegisteredClaimNames.Email, user.Email!),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            IList<string>? roles = await _userManager.GetRolesAsync(user);
            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: _signingCredentials);

            return _tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Generates a refresh token.
        /// </summary>
        /// <returns>The generated refresh token.</returns>
        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
