using NewsAggregatorCore;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class CronJobSettingFactory : ICronJobSettingFactory
    {
        readonly IEnumerable<ICronJobSetting> _jobSettings;

        IReadOnlyDictionary<string, Type> _jobTypeByKey => new Dictionary<string, Type>
        {
            [NewsAggregatorConstants.BeltaKey] = typeof(BeltaAggregationSetting),
            [NewsAggregatorConstants.TelegrafKey] = typeof(TelegrafAggregationSetting),
            [NewsAggregatorConstants.RiaKey] = typeof(RiaAggregationSetting),
            [NewsAggregatorConstants.NewsRaterKey] = typeof(NewsRatingSetting),
            [NewsAggregatorConstants.DelExpiredTokensKey] = typeof(DelExpiredRefreshTokenSetting)
        };

        public CronJobSettingFactory(IEnumerable<ICronJobSetting> jobSettings)
        {
            _jobSettings = jobSettings;
        }

        public ICronJobSetting? GetSettingByKey(string key)
        {
            _jobTypeByKey.TryGetValue(key, out Type? jobSettingType);
            return jobSettingType is null ? null : _jobSettings.FirstOrDefault(s => s.GetType() == jobSettingType);
        }
    }
}
