namespace OnlineShopPoC
{
    public class UtcTimeProvider : ITimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
