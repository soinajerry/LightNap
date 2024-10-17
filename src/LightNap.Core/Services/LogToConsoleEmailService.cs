using LightNap.Core.Identity;
using LightNap.Core.Interfaces;
using System.Diagnostics;
using System.Net.Mail;

namespace LightNap.Core.Services
{
    public class LogToConsoleEmailService : IEmailService
    {
        public Task SendEmailAsync(MailMessage message)
        {
            Trace.TraceInformation($"Not sending email to '{message.To.ToString()}' with subject '{message.Subject}' and body '{message.Body}'");
            return Task.CompletedTask;
        }

        public async Task SendPasswordResetEmailAsync(ApplicationUser user, string passwordResetUrl)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Reset your password", $"You may reset your password at: {passwordResetUrl}"));
        }

        public async Task SendRegistrationEmailAsync(ApplicationUser user)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Welcome to our site", $"Thank you for registering."));
        }

        public async Task SendTwoFactorEmailAsync(ApplicationUser user, string code)
        {
            await this.SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Your login security code", $"Your login code is: {code}"));
        }
    }
}
