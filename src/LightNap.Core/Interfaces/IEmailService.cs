using LightNap.Core.Identity;
using System.Net.Mail;

namespace LightNap.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailMessage message);
        Task SendTwoFactorEmailAsync(ApplicationUser user, string code);
        Task SendPasswordResetEmailAsync(ApplicationUser user, string passwordResetUrl);
        Task SendRegistrationEmailAsync(ApplicationUser user);
    }
}