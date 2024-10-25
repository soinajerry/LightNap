using LightNap.Core.Api;
using LightNap.Core.Identity.Dto.Request;
using LightNap.Core.Identity.Dto.Response;

namespace LightNap.Core.Identity.Interfaces
{
    /// <summary>  
    /// Provides methods to manage identity.  
    /// </summary>  
    public interface IIdentityService
    {
        Task<ApiResponseDto<LoginResultDto>> LogInAsync(LoginRequestDto requestDto);
        Task<ApiResponseDto<LoginResultDto>> RegisterAsync(RegisterRequestDto requestDto);
        Task<ApiResponseDto<bool>> LogOutAsync();
        Task<ApiResponseDto<bool>> ResetPasswordAsync(ResetPasswordRequestDto requestDto);
        Task<ApiResponseDto<string>> NewPasswordAsync(NewPasswordRequestDto requestDto);
        Task<ApiResponseDto<string>> VerifyCodeAsync(VerifyCodeRequestDto requestDto);
        Task<ApiResponseDto<string>> RefreshTokenAsync();
    }
}