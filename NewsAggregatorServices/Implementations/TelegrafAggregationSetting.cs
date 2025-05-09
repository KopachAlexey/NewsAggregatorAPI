using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class TelegrafAggregationSetting : ICronJobSetting
    {
        public string JobName => "AggregationNewsFromTelegraf";

        public string JobCron => "0 */2 * * *";
    }
}
