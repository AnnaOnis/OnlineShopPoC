namespace OnlineShopPoC
{
    public interface IEmailSender
    {
        public Task SendEmail(string recepientEmail, string subject, string message);
    }
}