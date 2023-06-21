namespace OnlineShopPoC
{
    public class AppStartedNotificatorBackgroundService : BackgroundService
    {
        private readonly IEmailSender _emailSender;

        public AppStartedNotificatorBackgroundService(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _emailSender.SendEmail("onischenko.anna11@gmail.com", "Приложение", "Приложение запущено");
        }
    }
}
