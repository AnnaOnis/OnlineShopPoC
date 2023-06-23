using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.Extensions.Options;

namespace OnlineShopPoC
{
    public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly SmtpClient _smtpClient = new();
        private readonly SmtpConfig _smtpConfig;

        public MailKitSmtpEmailSender(IOptionsSnapshot<SmtpConfig> options)
        {
            ArgumentNullException.ThrowIfNull(nameof(options));
            _smtpConfig = options.Value;
        }


        public async ValueTask DisposeAsync()
        {
            await _smtpClient.DisconnectAsync(true);
            _smtpClient.Dispose();
        }

        public async Task SendEmail(string recepientEmail, string subject, string message)
        {
            ArgumentNullException.ThrowIfNull(recepientEmail);
            ArgumentNullException.ThrowIfNull(subject);
            ArgumentNullException.ThrowIfNull(message);

            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(MailboxAddress.Parse(_smtpConfig.FromEmail));
            emailMessage.To.Add(MailboxAddress.Parse(recepientEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart()
            {
                Text = message
            };

            await EnsureConnectedAndAuthenticatedAsync();
            await _smtpClient.SendAsync(emailMessage);

        }

        private async Task EnsureConnectedAndAuthenticatedAsync()
        {
            if(!_smtpClient.IsConnected)
            {
                //await _smtpClient.ConnectAsync("smtp.beget.com", 25, false);
                await _smtpClient.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false);
            }

            if (!_smtpClient.IsAuthenticated)
            {
                //var password = Environment.GetEnvironmentVariable("smtp_password");
                //await _smtpClient.AuthenticateAsync("asp2023pv112@rodion-m.ru", password);
                await _smtpClient.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
            }

        }
    }
}
