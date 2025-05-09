using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class NewsRatingSetting : ICronJobSetting
    {
        public string JobName => "NewsRating";

        public string JobCron => "0 * * * *";
    }
}
