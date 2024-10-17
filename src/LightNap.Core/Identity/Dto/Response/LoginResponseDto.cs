namespace LightNap.Core.Identity.Dto.Response
{
    public class LoginResponseDto
    {
        public bool TwoFactorRequired { get; set; }
        public string? BearerToken { get; set; }
    }
}