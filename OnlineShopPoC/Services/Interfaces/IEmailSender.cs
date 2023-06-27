namespace OnlineShopPoC.Services
{
    public interface IEmailSender
    {
        public Task SendEmail(string recepientEmail, string subject, string message);
    }
}