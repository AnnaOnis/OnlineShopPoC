using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.Extensions.Options;

namespace OnlineShopPoC.Services
{
    public class MailKitSmtpEmailSender : IEmailSender, IAsyncDisposable
    {
        private readonly SmtpClient _smtpClient = new();
        private readonly SmtpConfig _smtpConfig;
        private readonly IConfiguration _configuration;
        private readonly string _password;

        public MailKitSmtpEmailSender(IOptionsSnapshot<SmtpConfig> options, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(nameof(options));
            _smtpConfig = options.Value;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _password = configuration.GetValue<string>("SmtpConfig:Password");
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
            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, false);
            }

            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync(_smtpConfig.UserName, _password);
            }

        }
    }
}
