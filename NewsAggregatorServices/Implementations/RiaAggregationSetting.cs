using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class RiaAggregationSetting : ICronJobSetting
    {
        public string JobName => "AggregationNewsFromRia";

        public string JobCron => "0 * * * *";
    }
}
