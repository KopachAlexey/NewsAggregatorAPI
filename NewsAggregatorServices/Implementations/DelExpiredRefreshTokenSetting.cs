using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class DelExpiredRefreshTokenSetting : ICronJobSetting
    {
        public string JobName => "Removing expired tokens";
        public string JobCron => "*/15 * * * *";
    }
}
