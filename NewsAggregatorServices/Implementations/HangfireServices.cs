using System.Linq.Expressions;
using NewsAggregatorServices.Abstracts;
using Hangfire;
using Hangfire.Storage;

namespace NewsAggregatorServices.Implementations
{
    public class HangfireServices : IBackgroundJobServices
    {
        readonly IRecurringJobManager _recurringJobManager;
        readonly JobStorage _jobStorage;

        public HangfireServices(IRecurringJobManager recurringJobManager, JobStorage jobStorage)
        {
            _recurringJobManager = recurringJobManager;
            _jobStorage = jobStorage;
        }

        public void AddCronJob(ICronJobSetting cronJobSetting, Expression<Func<Task>> cronJob)
        {
            _recurringJobManager.AddOrUpdate(
                cronJobSetting.JobName,
                cronJob,
                cronJobSetting.JobCron
                );
        }

        public string AddFireAndForgetJob(Expression<Func<Task>> cronJob)
        {
            return BackgroundJob.Enqueue(cronJob);
        }

        public bool CheckCronJobExists(string jobName)
        {
            using (var connection = _jobStorage.GetConnection())
            {
                var cronJobs = connection.GetRecurringJobs();
                return cronJobs.Any(j => j.Id == jobName);
            }
        }

        public void DelCronJob(string jobName)
        {
            _recurringJobManager.RemoveIfExists(jobName);
        }
    }
}
