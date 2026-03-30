using ASP.NET_IDENTITY.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ASP.NET_IDENTITY.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SmtpSetting> smtpSetting;

        public EmailService(IOptions<SmtpSetting> smtpSetting)
        {
            this.smtpSetting = smtpSetting;
        }

        public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body)
        {
            var message = new MailMessage(
                    fromEmail,
                    toEmail,
                    subject,
                    body);

            using (var smtpClient = new SmtpClient(smtpSetting.Value.Host, smtpSetting.Value.Port))
            {
                // Konfiguracja SMTP (np. port, uwierzytelnianie)
                smtpClient.Credentials = new NetworkCredential(smtpSetting.Value.User, smtpSetting.Value.Password);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
