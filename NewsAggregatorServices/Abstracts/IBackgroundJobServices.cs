using System.Linq.Expressions;

namespace NewsAggregatorServices.Abstracts
{
    public interface IBackgroundJobServices
    {
        public void AddCronJob(ICronJobSetting cronJobSetting, Expression<Func<Task>> cronJob);

        public void DelCronJob(string jobName);

        public bool CheckCronJobExists(string jobName);

        public string AddFireAndForgetJob(Expression<Func<Task>> cronJob);
    }
}
