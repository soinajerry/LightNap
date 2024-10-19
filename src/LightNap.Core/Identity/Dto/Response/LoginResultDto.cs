namespace LightNap.Core.Identity.Dto.Response
{
    public class LoginResultDto
    {
        public bool TwoFactorRequired { get; set; }
        public string? BearerToken { get; set; }
    }
}