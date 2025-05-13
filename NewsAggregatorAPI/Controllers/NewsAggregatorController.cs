using Microsoft.AspNetCore.Mvc;
using NewsAggregatorServices.Abstracts;
using Microsoft.AspNetCore.Authorization;
using NewsAggregatorCore.DTO;


namespace NewsAggregatorAPI.Controllers
{
    [Authorize(Roles = "Developer, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsAggregatorController : ControllerBase
    {
        readonly INewsAggregator _newsAggregator;
        readonly ISourceServices _sourceServices;
        readonly IBackgroundJobServices _backgroundJobServices;
        readonly ICronJobSettingFactory _newsAggregationJobSettingFactory;
        readonly ILogger<NewsAggregatorController> _logger;

        public NewsAggregatorController(INewsAggregator newsAggregator, ISourceServices sourceServices, 
            IBackgroundJobServices backgroundJobServices, ICronJobSettingFactory newsAggregationJobSettingFactory, 
            ILogger<NewsAggregatorController> logger)
        {
            _newsAggregator = newsAggregator;
            _sourceServices = sourceServices;
            _backgroundJobServices = backgroundJobServices;
            _newsAggregationJobSettingFactory = newsAggregationJobSettingFactory;
            _logger = logger;
        }

        [HttpPost("start-aggregation")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StartNewsAggregation()
        {
            try
            {
                var sources = await _sourceServices.GetAllAsync();
                foreach (var source in sources)
                {
                    var jobSetting = _newsAggregationJobSettingFactory.GetSettingByKey(source.Name);
                    if (jobSetting is null)
                        continue;
                    _backgroundJobServices.AddCronJob(
                        jobSetting,
                        () => _newsAggregator.AggregateNewsFromSourceAsync(source)
                        );
                }
                return Accepted();
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to start news aggregation");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpPost("stop-aggregation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> StopNewsAggregation()
        {
            try
            {
                var sources = await _sourceServices.GetAllAsync();
                foreach (var source in sources)
                {
                    var jobSetting = _newsAggregationJobSettingFactory.GetSettingByKey(source.Name);
                    if (jobSetting is null)
                        continue;
                    _backgroundJobServices.DelCronJob(jobSetting.JobName);
                }
                return Ok();
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to stopp news aggregation");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("get-aggregation-jobs")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CronJobDTO[]))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNewsAggregationJobs()
        {
            try
            {
                var aggregationJobs = new List<CronJobDTO>();
                var sources = await _sourceServices.GetAllAsync();
                foreach (var source in sources)
                {
                    var jobSetting = _newsAggregationJobSettingFactory.GetSettingByKey(source.Name);
                    if (jobSetting is null)
                        continue;
                    aggregationJobs.Add(new CronJobDTO
                    {
                        Name = jobSetting.JobName,
                        IsRunning = _backgroundJobServices.CheckCronJobExists(jobSetting.JobName)
                    });
                }
                return Ok(aggregationJobs.ToArray());
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to get all news aggregation works");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("aggregate-once")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AggregateNewsOnce()
        {
            try
            {
                var sources = await _sourceServices.GetAllAsync();
                foreach (var source in sources)
                    _backgroundJobServices.AddFireAndForgetJob(() => _newsAggregator.AggregateNewsFromSourceAsync(source));
                return Accepted();
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to aggregate news once");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
