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
            this._configuration = configuration;
            this._userManager = userManager;

            var tokenKey = this._configuration.GetRequiredSetting("Jwt:Key");
            if (tokenKey.Length < 32) { throw new ArgumentException("The provided setting 'Jwt:Key' must be at least 32 characters long"); }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            this._signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            this._issuer = this._configuration.GetRequiredSetting("Jwt:Issuer");
            this._audience = this._configuration.GetRequiredSetting("Jwt:Audience");
            this._expirationMinutes = int.Parse(this._configuration.GetRequiredSetting("Jwt:ExpirationMinutes"));
            this._tokenHandler = new JwtSecurityTokenHandler();
        }

        /// <summary>
        /// Generates an access token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom to generate the token.</param>
        /// <returns>The generated access token.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the user parameter is null.</exception>
        public async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new(JwtRegisteredClaimNames.Email, user.Email!),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            IList<string>? roles = await this._userManager.GetRolesAsync(user);
            if ((roles != null) && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var token = new JwtSecurityToken(
                issuer: this._issuer,
                audience: this._audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(this._expirationMinutes),
                signingCredentials: this._signingCredentials);

            return this._tokenHandler.WriteToken(token);
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
