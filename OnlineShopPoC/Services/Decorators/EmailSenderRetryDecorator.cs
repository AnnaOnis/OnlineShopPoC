using OnlineShopPoC.Services;
using Polly.Retry;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace OnlineShopPoC.Decorators
{
    public class EmailSenderRetryDecorator : IEmailSender
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly int _attemptsLimit;

        public EmailSenderRetryDecorator(
                    IEmailSender emailSender,
                    ILogger<EmailSenderRetryDecorator> logger,
                    IConfiguration configuration)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _attemptsLimit = _configuration.GetValue<int>("EmailRetryCount");
        }

        public async Task SendEmail(string recepientEmail, string subject, string message)
        {
            var delay = Backoff.ExponentialBackoff(TimeSpan.FromMilliseconds(200), retryCount: _attemptsLimit-1);

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(delay, onRetry: (exception, retryAttempt) =>
                {
                    _logger.LogWarning(
                        exception, "Ошибка при отправке Email. Повторная попытка: {Attempt}", retryAttempt);
                });

            PolicyResult? result = await retryPolicy.ExecuteAndCaptureAsync(
                                           () => _emailSender.SendEmail(recepientEmail, subject, message));

            if (result.Outcome == OutcomeType.Failure)
            {
                _logger.LogError(result.FinalException, "Произошла ошибка при отправке email!");
            }
        }
    }
}
