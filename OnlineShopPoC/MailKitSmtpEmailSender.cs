using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace OnlineShopPoC
{
    public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly SmtpClient _smtpClient = new();


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

            emailMessage.From.Add(MailboxAddress.Parse("asp2023pv112@rodion-m.ru"));
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
                await _smtpClient.ConnectAsync("smtp.beget.com", 25, false);
            }

            if (!_smtpClient.IsAuthenticated)
            {
                var password = Environment.GetEnvironmentVariable("smtp_password");
                await _smtpClient.AuthenticateAsync("asp2023pv112@rodion-m.ru", password);
            }

        }
    }
}
