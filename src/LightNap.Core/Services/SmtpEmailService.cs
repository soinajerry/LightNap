using LightNap.Core.Extensions;
using LightNap.Core.Identity;
using LightNap.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace LightNap.Core.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly string _fromDisplayName;

        public SmtpEmailService(IConfiguration configuration)
        {
            this._smtpClient = new SmtpClient(configuration.GetRequiredSetting("Smtp:Host"), int.Parse(configuration.GetRequiredSetting("Smtp:Port")))
            {
                Credentials = new NetworkCredential(configuration.GetRequiredSetting("Smtp:User"), configuration.GetRequiredSetting("Smtp:Password")),
                EnableSsl = bool.Parse(configuration.GetRequiredSetting("Smtp:EnableSsl"))
            };

            this._fromEmail = configuration.GetRequiredSetting("Smtp:FromEmail");
            this._fromDisplayName = configuration.GetRequiredSetting("Smtp:FromDisplayName");
        }

        public async Task SendEmailAsync(MailMessage message)
        {
            message.From = new MailAddress(this._fromEmail, this._fromDisplayName);
            await this._smtpClient.SendMailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(ApplicationUser user, string passwordResetUrl)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, user.Email!, "Reset your password", $"You may reset your password at: {passwordResetUrl}"));
        }

        public async Task SendRegistrationEmailAsync(ApplicationUser user)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, user.Email!, "Welcome to our site", $"Thank you for registering."));
        }

        public async Task SendTwoFactorEmailAsync(ApplicationUser user, string code)
        {
            await this.SendEmailAsync(new MailMessage(this._fromEmail, user.Email!, "Your login security code", $"Your login code is: {code}"));
        }
    }
}
