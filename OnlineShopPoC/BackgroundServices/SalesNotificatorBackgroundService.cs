using Microsoft.Extensions.Configuration;
using OnlineShopPoC.Services;
using System.Diagnostics;

namespace OnlineShopPoC.BackgroundServices
{
    public class SalesNotificatorBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SalesNotificatorBackgroundService> _logger;
        private readonly IConfiguration _configuration;
        private readonly int _attemptsLimit;

        public SalesNotificatorBackgroundService(IServiceProvider serviceProvider,
                ILogger<SalesNotificatorBackgroundService> logger,
                IConfiguration configuration)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _attemptsLimit = _configuration.GetValue<int>("EmailRetryCount");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var localServiceProvider = scope.ServiceProvider;
            var emailSender = localServiceProvider.GetRequiredService<IEmailSender>();

            var users = new User[]
            {
                new("onischenko.anna11@gmail.com"),
                new("onischenko.anna11@gmail.com"),
            };

            foreach (var user in users)
            {
                for (var attempt = 1; attempt <= _attemptsLimit; attempt++)
                {
                    try
                    {
                        await emailSender.SendEmail(user.Email, "Промоакции", "Список акций");
                        break;
                    }
                    catch (Exception ex) when (attempt < _attemptsLimit)
                    {
                        _logger.LogWarning(ex, "Повторная отправка сообщения: {Email} номер {count}", user.Email, attempt + 1);
                        await Task.Delay(1000);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка отправки сообщения: {Email}", user.Email);
                        await Task.Delay(1000);
                    }
                }
            }
        }
    }

    public record User(string Email);
}
