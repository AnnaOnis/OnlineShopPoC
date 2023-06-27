namespace OnlineShopPoC.BackgroundServices
{
    public class AppStartedNotificatorBackgroundService : BackgroundService
    {
        //private readonly IEmailSender _emailSender;
        private readonly ILogger<AppStartedNotificatorBackgroundService> _logger;

        public AppStartedNotificatorBackgroundService(
                        ILogger<AppStartedNotificatorBackgroundService> logger)
        {
            //_emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Сервер успешно запущен!");
            //await _emailSender.SendEmail("onischenko.anna11@gmail.com", "Приложение", "Приложение запущено");
        }
    }
}
