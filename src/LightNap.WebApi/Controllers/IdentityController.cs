using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Identity;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Identity.Dto.Response;
using LightNap.Core.Interfaces;
using LightNap.WebApi.Configuration;
using LightNap.WebApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Web;

namespace LightNap.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly SiteSettings _siteSettings;
        private readonly ApplicationDbContext _db;

        public IdentityController(ILogger<IdentityController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, ITokenService tokenService, IEmailService emailService, IOptions<SiteSettings> siteSettings, ApplicationDbContext db)
        {
            this._logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._tokenService = tokenService;
            this._emailService = emailService;
            this._siteSettings = siteSettings.Value;
            this._db = db;
        }

        private async Task<ApiResponseDto<LoginResultDto>> HandleUserLoginAsync(ApplicationUser user, bool rememberMe, string deviceDetails)
        {
            if (user.TwoFactorEnabled)
            {
                string code = await this._userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

                try
                {
                    await this._emailService.SendTwoFactorEmailAsync(user, code);
                }
                catch (Exception e)
                {
                    this._logger.LogError(e, $"An error occurred while sending a 2FA code to '{user.Email}'.");
                    return ApiResponseDto<LoginResultDto>.CreateError("An unexpected error occurred while sending the 2FA code.");
                }

                return ApiResponseDto<LoginResultDto>.CreateSuccess(new LoginResultDto() { TwoFactorRequired = true });
            }

            await this.CreateRefreshTokenAsync(user, rememberMe, deviceDetails);

            return ApiResponseDto<LoginResultDto>.CreateSuccess(
                new LoginResultDto()
                {
                    BearerToken = await this._tokenService.GenerateAccessTokenAsync(user)
                });
        }

        private async Task<ApplicationUser?> ValidateRefreshTokenAsync()
        {
            string? refreshTokenCookie = this.Request.Cookies[Constants.Cookies.RefreshToken];
            if (refreshTokenCookie is null) { return null; }

            var refreshToken = await this._db.RefreshTokens.Include(token => token.User).FirstOrDefaultAsync(token => token.Token == refreshTokenCookie);
            if (refreshToken is null || refreshToken.IsRevoked || (refreshToken.Expires < DateTime.UtcNow)) { return null; }

            refreshToken.LastSeen = DateTime.UtcNow;
            refreshToken.IpAddress = this.Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            refreshToken.Expires = DateTime.UtcNow.AddDays(30);
            refreshToken.Token = this._tokenService.GenerateRefreshToken();

            await this._db.SaveChangesAsync();

            bool rememberMe = refreshTokenCookie.Contains("Expires") || refreshTokenCookie.Contains("Max-Age");

            this.SetRefreshTokenCookieAsync(refreshToken.Token, rememberMe, refreshToken.Expires);

            return refreshToken.User;
        }

        private async Task CreateRefreshTokenAsync(ApplicationUser user, bool rememberMe, string deviceDetails)
        {
            DateTime expires = DateTime.UtcNow.AddDays(30);
            string refreshToken = this._tokenService.GenerateRefreshToken();

            this._db.RefreshTokens.Add(
                new RefreshToken()
                {
                    Id = Guid.NewGuid().ToString(),
                    Token = refreshToken,
                    Expires = expires,
                    LastSeen = DateTime.UtcNow,
                    IpAddress = this.Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    Details = deviceDetails,
                    UserId = user.Id
                });
            await this._db.SaveChangesAsync();

            this.SetRefreshTokenCookieAsync(refreshToken, rememberMe, expires);
        }

        private void SetRefreshTokenCookieAsync(string refreshToken, bool rememberMe, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = this._siteSettings.UseSameSiteStrictCookies ? SameSiteMode.Strict : SameSiteMode.None,
            };

            if (rememberMe)
            {
                cookieOptions.Expires = expires;
            }

            this.Response.Cookies.Append(Constants.Cookies.RefreshToken, refreshToken, cookieOptions);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponseDto<LoginResultDto>>> LogIn([FromBody] LoginRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.Email) || string.IsNullOrEmpty(requestDto.Password)) { return this.BadRequest(); }

            ApplicationUser? user = await this._userManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                return ApiResponseDto<LoginResultDto>.CreateError("Invalid email/password combination.");
            }

            var signInResult = await this._signInManager.CheckPasswordSignInAsync(user, requestDto.Password, false);
            if (!signInResult.Succeeded)
            {
                if (signInResult.IsLockedOut)
                {
                    return ApiResponseDto<LoginResultDto>.CreateError("This account is locked.");
                }
                if (signInResult.IsNotAllowed)
                {
                    return ApiResponseDto<LoginResultDto>.CreateError("This account is not allowed to log in.");
                }
                return ApiResponseDto<LoginResultDto>.CreateError("Invalid email/password combination.");
            }

            return await this.HandleUserLoginAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponseDto<LoginResultDto>>> Register([FromBody] RegisterRequestDto requestDto)
        {
            if (string.IsNullOrEmpty(requestDto.Email) || string.IsNullOrEmpty(requestDto.Password)) { return this.BadRequest(); }

            var userExists = await this._userManager.FindByEmailAsync(requestDto.Email);
            if (userExists != null)
            {
                return ApiResponseDto<LoginResultDto>.CreateError("This email is already in use.");
            }

            ApplicationUser user = new()
            {
                Email = requestDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                TwoFactorEnabled = this._siteSettings.RequireTwoFactorForNewUsers,
                UserName = requestDto.UserName
            };

            var result = await this._userManager.CreateAsync(user, requestDto.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { return ApiResponseDto<LoginResultDto>.CreateError(result.Errors.Select(item => item.Description).ToArray()); }
                return ApiResponseDto<LoginResultDto>.CreateError("Unable to create user.");
            }

            if (!user.TwoFactorEnabled)
            {
                await this._emailService.SendRegistrationEmailAsync(user);
            }

            return await this.HandleUserLoginAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);
        }

        [HttpGet("logout")]
        public async Task<ActionResult<ApiResponseDto<bool>>> LogOut()
        {
            string? refreshTokenCookie = this.Request.Cookies[Constants.Cookies.RefreshToken];
            if (refreshTokenCookie is not null)
            {
                RefreshToken? refreshToken = await this._db.RefreshTokens.FirstOrDefaultAsync(token => token.Token == refreshTokenCookie);
                if (refreshToken is not null)
                {
                    this._db.RefreshTokens.Remove(refreshToken);
                    await this._db.SaveChangesAsync();
                }
                this.Response.Cookies.Delete(Constants.Cookies.RefreshToken);
            }
            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<ApiResponseDto<bool>>> ResetPassword(ResetPasswordRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.Email)) { return this.BadRequest(); }

            ApplicationUser? user = await this._userManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                return ApiResponseDto<bool>.CreateError("An account with this email was not found.");
            }

            string token = await this._userManager.GeneratePasswordResetTokenAsync(user);
            string url = $"{this._siteSettings.SiteUrlRootForEmails}#/auth/new-password/{HttpUtility.UrlEncode(user.Email)}/{HttpUtility.UrlEncode(token)}";

            try
            {
                await this._emailService.SendPasswordResetEmailAsync(user, url);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"An error occurred while sending a password reset link to '{user.Email}'");
                return ApiResponseDto<bool>.CreateError("An unexpected error occurred while sending the password reset link.");
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        [HttpPost("new-password")]
        public async Task<ActionResult<ApiResponseDto<string>>> NewPassword(NewPasswordRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.Email)) { return this.BadRequest(); }

            ApplicationUser? user = await this._userManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                return ApiResponseDto<string>.CreateError("An account with this email was not found.");
            }

            IdentityResult result = await this._userManager.ResetPasswordAsync(user, requestDto.Token, requestDto.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { return ApiResponseDto<string>.CreateError(result.Errors.Select(item => item.Description).ToArray()); }
                return ApiResponseDto<string>.CreateError("Unable to set new password.");
            }

            await this.CreateRefreshTokenAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);

            return ApiResponseDto<string>.CreateSuccess(await this._tokenService.GenerateAccessTokenAsync(user));
        }


        [HttpPost("verify-code")]
        public async Task<ActionResult<ApiResponseDto<string>>> VerifyCode(VerifyCodeRequestDto requestDto)
        {
            if (string.IsNullOrWhiteSpace(requestDto.Email) || string.IsNullOrWhiteSpace(requestDto.Code)) { return this.BadRequest(); }

            ApplicationUser? user = await this._userManager.FindByEmailAsync(requestDto.Email);
            if ((user is null) || !(await this._userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, requestDto.Code)))
            {
                return ApiResponseDto<string>.CreateError("Unable to verify code. Please try again or log in again to resend a new code.");
            }

            if (!user!.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await this._userManager.UpdateAsync(user);
                await this._emailService.SendRegistrationEmailAsync(user);
            }

            await this.CreateRefreshTokenAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);

            return ApiResponseDto<string>.CreateSuccess(await this._tokenService.GenerateAccessTokenAsync(user));
        }

        [HttpGet("refresh-token")]
        public async Task<ActionResult<ApiResponseDto<string>>> RefreshToken()
        {
            var user = await this.ValidateRefreshTokenAsync();

            if (user is null) { return ApiResponseDto<string>.CreateError("This account needs to sign in."); }

            if (!await this._signInManager.CanSignInAsync(user)) { return ApiResponseDto<string>.CreateError("This account may not sign in."); }

            return ApiResponseDto<string>.CreateSuccess(await this._tokenService.GenerateAccessTokenAsync(user));
        }
    }
}
