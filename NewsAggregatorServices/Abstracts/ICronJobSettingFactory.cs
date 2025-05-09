namespace NewsAggregatorServices.Abstracts
{
    public interface ICronJobSettingFactory
    {
        ICronJobSetting? GetSettingByKey(string key);
    }
}
