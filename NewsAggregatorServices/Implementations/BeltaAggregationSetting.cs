using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class BeltaAggregationSetting : ICronJobSetting
    {
        public string JobName => "AggregationNewsFromBelta";

        public string JobCron => "0 * * * *";
    }
}
