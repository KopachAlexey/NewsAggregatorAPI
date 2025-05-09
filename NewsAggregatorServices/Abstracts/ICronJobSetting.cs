
namespace NewsAggregatorServices.Abstracts
{
    public interface ICronJobSetting
    {
        string JobName { get; }
        string JobCron { get; }
    }
}
