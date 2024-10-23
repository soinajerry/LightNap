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
    /// <summary>
    /// Controller for handling identity-related actions such as login, registration, and password reset.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController(
        ILogger<IdentityController> logger,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IEmailService emailService,
        IOptions<SiteSettings> siteSettings,
        ApplicationDbContext db) : ControllerBase
    {
        /// <summary>
        /// Handles user login asynchronously.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="rememberMe">Indicates whether to remember the user.</param>
        /// <param name="deviceDetails">The device details.</param>
        /// <returns>The API response containing the login result.</returns>
        private async Task<ApiResponseDto<LoginResultDto>> HandleUserLoginAsync(ApplicationUser user, bool rememberMe, string deviceDetails)
        {
            try
            {
                if (user.TwoFactorEnabled)
                {
                    string code = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
                    await emailService.SendTwoFactorEmailAsync(user, code);
                    return ApiResponseDto<LoginResultDto>.CreateSuccess(new LoginResultDto() { TwoFactorRequired = true });
                }

                await this.CreateRefreshTokenAsync(user, rememberMe, deviceDetails);
                return ApiResponseDto<LoginResultDto>.CreateSuccess(
                    new LoginResultDto()
                    {
                        BearerToken = await tokenService.GenerateAccessTokenAsync(user)
                    });
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred during user login.");
                return ApiResponseDto<LoginResultDto>.CreateError("An unexpected error occurred during login.");
            }
        }

        /// <summary>
        /// Validates the refresh token asynchronously.
        /// </summary>
        /// <returns>The application user if the refresh token is valid; otherwise, null.</returns>
        private async Task<ApplicationUser?> ValidateRefreshTokenAsync()
        {
            string? refreshTokenCookie = this.Request.Cookies[Constants.Cookies.RefreshToken];
            if (refreshTokenCookie is null) { return null; }

            var refreshToken = await db.RefreshTokens.Include(token => token.User).FirstOrDefaultAsync(token => token.Token == refreshTokenCookie);
            if (refreshToken is null || refreshToken.IsRevoked || (refreshToken.Expires < DateTime.UtcNow)) { return null; }

            refreshToken.LastSeen = DateTime.UtcNow;
            refreshToken.IpAddress = this.Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? Constants.RefreshTokens.NoIpProvided;
            refreshToken.Expires = DateTime.UtcNow.AddDays(30);
            refreshToken.Token = tokenService.GenerateRefreshToken();

            await db.SaveChangesAsync();

            // If neither of these was set last time then the user doesn't want us to remember them across sessions.
            bool rememberMe = refreshTokenCookie.Contains(Constants.Cookies.Expires) || refreshTokenCookie.Contains(Constants.Cookies.MaxAge);

            this.SetRefreshTokenCookie(refreshToken.Token, rememberMe, refreshToken.Expires);

            return refreshToken.User;
        }

        /// <summary>
        /// Creates a refresh token asynchronously.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="rememberMe">Indicates whether to remember the user.</param>
        /// <param name="deviceDetails">The device details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task CreateRefreshTokenAsync(ApplicationUser user, bool rememberMe, string deviceDetails)
        {
            DateTime expires = DateTime.UtcNow.AddDays(30);
            string refreshToken = tokenService.GenerateRefreshToken();

            db.RefreshTokens.Add(
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
            await db.SaveChangesAsync();

            this.SetRefreshTokenCookie(refreshToken, rememberMe, expires);
        }

        /// <summary>
        /// Sets the refresh token cookie.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="rememberMe">Indicates whether to remember the user.</param>
        /// <param name="expires">The expiration date of the refresh token.</param>
        private void SetRefreshTokenCookie(string refreshToken, bool rememberMe, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = siteSettings.Value.UseSameSiteStrictCookies ? SameSiteMode.Strict : SameSiteMode.None,
            };

            if (rememberMe)
            {
                cookieOptions.Expires = expires;
            }

            this.Response.Cookies.Append(Constants.Cookies.RefreshToken, refreshToken, cookieOptions);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="requestDto">The login request DTO.</param>
        /// <returns>The API response containing the login result.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResultDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<LoginResultDto>>> LogIn([FromBody] LoginRequestDto requestDto)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                return ApiResponseDto<LoginResultDto>.CreateError("Invalid email/password combination.");
            }

            var signInResult = await signInManager.CheckPasswordSignInAsync(user, requestDto.Password, false);
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="requestDto">The registration request DTO.</param>
        /// <returns>The API response containing the login result.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponseDto<LoginResultDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<LoginResultDto>>> Register([FromBody] RegisterRequestDto requestDto)
        {
            var userExists = await userManager.FindByEmailAsync(requestDto.Email);
            if (userExists != null)
            {
                return ApiResponseDto<LoginResultDto>.CreateError("This email is already in use.");
            }

            ApplicationUser user = new()
            {
                Email = requestDto.Email,
                TwoFactorEnabled = siteSettings.Value.RequireTwoFactorForNewUsers,
                UserName = requestDto.UserName,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
            };

            var result = await userManager.CreateAsync(user, requestDto.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { return ApiResponseDto<LoginResultDto>.CreateError(result.Errors.Select(item => item.Description).ToArray()); }
                return ApiResponseDto<LoginResultDto>.CreateError("Unable to create user.");
            }

            if (!user.TwoFactorEnabled)
            {
                await emailService.SendRegistrationEmailAsync(user);
            }

            logger.LogInformation("New user '{userName}' ('{email}') registered!", user.Email, user.UserName);

            return await this.HandleUserLoginAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>The API response indicating the success of the operation.</returns>
        [HttpGet("logout")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        public async Task<ActionResult<ApiResponseDto<bool>>> LogOut()
        {
            string? refreshTokenCookie = this.Request.Cookies[Constants.Cookies.RefreshToken];
            if (refreshTokenCookie is not null)
            {
                RefreshToken? refreshToken = await db.RefreshTokens.FirstOrDefaultAsync(token => token.Token == refreshTokenCookie);
                if (refreshToken is not null)
                {
                    db.RefreshTokens.Remove(refreshToken);
                    await db.SaveChangesAsync();
                }
                this.Response.Cookies.Delete(Constants.Cookies.RefreshToken);
            }
            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// Resets the password for a user.
        /// </summary>
        /// <param name="requestDto">The reset password request DTO.</param>
        /// <returns>The API response indicating the success of the operation.</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<bool>>> ResetPassword(ResetPasswordRequestDto requestDto)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                return ApiResponseDto<bool>.CreateError("An account with this email was not found.");
            }

            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            string url = $"{siteSettings.Value.SiteUrlRootForEmails}#/auth/new-password/{HttpUtility.UrlEncode(user.Email)}/{HttpUtility.UrlEncode(token)}";

            try
            {
                await emailService.SendPasswordResetEmailAsync(user, url);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while sending a password reset link to '{email}'", user.Email);
                return ApiResponseDto<bool>.CreateError("An unexpected error occurred while sending the password reset link.");
            }

            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        /// <summary>
        /// Sets a new password for a user.
        /// </summary>
        /// <param name="requestDto">The new password request DTO.</param>
        /// <returns>The API response containing the new access token.</returns>
        [HttpPost("new-password")]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<string>>> NewPassword(NewPasswordRequestDto requestDto)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(requestDto.Email);
            if (user == null)
            {
                return ApiResponseDto<string>.CreateError("An account with this email was not found.");
            }

            IdentityResult result = await userManager.ResetPasswordAsync(user, requestDto.Token, requestDto.Password);
            if (!result.Succeeded)
            {
                if (result.Errors.Any()) { return ApiResponseDto<string>.CreateError(result.Errors.Select(item => item.Description).ToArray()); }
                return ApiResponseDto<string>.CreateError("Unable to set new password.");
            }

            await this.CreateRefreshTokenAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);

            return ApiResponseDto<string>.CreateSuccess(await tokenService.GenerateAccessTokenAsync(user));
        }

        /// <summary>
        /// Verifies the two-factor authentication code.
        /// </summary>
        /// <param name="requestDto">The verify code request DTO.</param>
        /// <returns>The API response containing the new access token.</returns>
        [HttpPost("verify-code")]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ApiResponseDto<string>>> VerifyCode(VerifyCodeRequestDto requestDto)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(requestDto.Email);
            if ((user is null) || !(await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, requestDto.Code)))
            {
                return ApiResponseDto<string>.CreateError("Unable to verify code. Please try again or log in again to resend a new code.");
            }

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);
                await emailService.SendRegistrationEmailAsync(user);
            }

            await this.CreateRefreshTokenAsync(user, requestDto.RememberMe, requestDto.DeviceDetails);

            return ApiResponseDto<string>.CreateSuccess(await tokenService.GenerateAccessTokenAsync(user));
        }

        /// <summary>
        /// Refreshes the access token using the refresh token.
        /// </summary>
        /// <returns>The API response containing the new access token.</returns>
        [HttpGet("access-token")]
        [ProducesResponseType(typeof(ApiResponseDto<string>), 200)]
        public async Task<ActionResult<ApiResponseDto<string>>> RefreshToken()
        {
            var user = await this.ValidateRefreshTokenAsync();

            if (user is null) { return ApiResponseDto<string>.CreateError("This account needs to sign in."); }

            if (!await signInManager.CanSignInAsync(user)) { return ApiResponseDto<string>.CreateError("This account may not sign in."); }

            return ApiResponseDto<string>.CreateSuccess(await tokenService.GenerateAccessTokenAsync(user));
        }
    }
}
