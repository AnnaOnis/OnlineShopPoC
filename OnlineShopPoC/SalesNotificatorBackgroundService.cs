using System.Diagnostics;

namespace OnlineShopPoC
{
    public class SalesNotificatorBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SalesNotificatorBackgroundService> _logger;
        private readonly ITimeProvider _timeProvider;

        public SalesNotificatorBackgroundService(IServiceProvider serviceProvider,
                ILogger<SalesNotificatorBackgroundService> logger,
                ITimeProvider timeProvider)
        {
            _serviceProvider= serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(logger)); ;
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

            var sw = Stopwatch.StartNew();
            foreach (var user in users)
            {
                sw.Restart();
                await emailSender.SendEmail(user.Email, "Промоакции", "Список акций");
                _logger.LogInformation($"Email sent to {user.Email} in {sw.ElapsedMilliseconds} ms");
                //Email sent to onischenko.anna11@gmail.com in 872 ms / 610 ms
                //Email sent to onischenko.anna11@gmail.com in 87 ms / 76 ms
            }
        }
    }

    public record User(string Email);
}
