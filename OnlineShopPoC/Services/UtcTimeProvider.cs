
namespace OnlineShopPoC.Services
{
    public class UtcTimeProvider : ITimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
