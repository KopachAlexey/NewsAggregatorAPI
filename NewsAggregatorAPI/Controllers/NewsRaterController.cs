using NewsAggregatorCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorServices.Abstracts;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorAPI.Controllers
{
    [Authorize(Roles = "Developer, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsRaterController : ControllerBase
    {
        readonly INewsRater _newsRater;
        readonly ICronJobSettingFactory _cronJobSettingFactory;
        readonly IBackgroundJobServices _backgroundJobServices;
        readonly ILogger<NewsRaterController> _logger;

        public NewsRaterController(INewsRater newsRater, ICronJobSettingFactory cronJobSettingFactory, 
            IBackgroundJobServices backgroundJobServices, ILogger<NewsRaterController> logger)
        {
            _newsRater = newsRater;
            _cronJobSettingFactory = cronJobSettingFactory;
            _backgroundJobServices = backgroundJobServices;
            _logger = logger;
        }

        [HttpPost("rate-once")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RateNewsOnce()
        {
            try
            {
                _backgroundJobServices.AddFireAndForgetJob(() => _newsRater.RateNewsAsync(default));
                return Accepted();
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to rate news once");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        [HttpPost("start-rating")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StartNewsRating()
        {
            try
            {
                var newsRatingSetting = _cronJobSettingFactory.GetSettingByKey(NewsAggregatorConstants.NewsRaterKey);
                if (newsRatingSetting is null)
                    throw new ArgumentException("There is no setting for news rating job");
                _backgroundJobServices.AddCronJob(
                    newsRatingSetting,
                    () => _newsRater.RateNewsAsync(default)
                    );
                return Accepted();
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to start news rating");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }

        [HttpGet("get-rating-job")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CronJobDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNewsRatingJob()
        {
            try
            {
                var newsRatingSetting = _cronJobSettingFactory.GetSettingByKey(NewsAggregatorConstants.NewsRaterKey);
                if (newsRatingSetting is null)
                    throw new ArgumentException("There is no setting for news rating job");
                var ratingJob = new CronJobDTO
                {
                    Name = newsRatingSetting.JobName,
                    IsRunning = _backgroundJobServices.CheckCronJobExists(newsRatingSetting.JobName)
                };
                return Ok(ratingJob);
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to get all news rating jobs");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("stop-rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StopNewsRating()
        {
            try
            {
                var newsRatingSetting = _cronJobSettingFactory.GetSettingByKey(NewsAggregatorConstants.NewsRaterKey);
                if (newsRatingSetting is null)
                    throw new ArgumentException("There is no such rating setting for this job");
                _backgroundJobServices.DelCronJob(newsRatingSetting.JobName);
                return Ok();
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to stop news rating");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
