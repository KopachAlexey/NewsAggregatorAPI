using NewsAggregatorCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorServices.Abstracts;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Implementations;

namespace NewsAggregatorAPI.Controllers
{
    [Authorize(Roles = "Developer, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokensCleanerController : ControllerBase
    {
        readonly IBackgroundJobServices _backgroundJobServices;
        readonly ITokenServices _tokenServices;
        readonly ICronJobSettingFactory _newsAggregationJobSettingFactory;

        public TokensCleanerController(IBackgroundJobServices backgroundJobServices, ITokenServices tokenServices, 
            ICronJobSettingFactory newsAggregationJobSettingFactory)
        {
            _backgroundJobServices = backgroundJobServices;
            _tokenServices = tokenServices;
            _newsAggregationJobSettingFactory = newsAggregationJobSettingFactory;
        }

        [HttpPost("start-deletion-expired-tokens")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StartDeletionExpiredTokens()
        {
            try
            {
                var delTokensSetting = _newsAggregationJobSettingFactory.GetSettingByKey(NewsAggregatorConstants.DelExpiredTokensKey);
                if (delTokensSetting is null)
                    throw new ArgumentException("There is no setting for expired refresh token deleted job");
                _backgroundJobServices.AddCronJob(
               delTokensSetting,
               () => _tokenServices.DelExpiredRefreshTokensAsync()
               );
                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }

        [HttpPost("stop-deletion-expired-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StopDeletionExpiredTokens()
        {
            try
            {
                var delTokensSetting = _newsAggregationJobSettingFactory
                    .GetSettingByKey(NewsAggregatorConstants.DelExpiredTokensKey);
                if (delTokensSetting is null)
                    throw new ArgumentException("There is no setting for expired refresh token deleted job");
                _backgroundJobServices.DelCronJob(delTokensSetting.JobName);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet("get-tokens-cleaner-job")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CronJobDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetTokensCleanerJob()
        {
            try
            {
                var delTokensSetting = _newsAggregationJobSettingFactory
                    .GetSettingByKey(NewsAggregatorConstants.DelExpiredTokensKey);
                if (delTokensSetting is null)
                    throw new ArgumentException("There is no setting for expired refresh token deleted job");
                var delTokensJob = new CronJobDTO
                {
                    Name = delTokensSetting.JobName,
                    IsRunning = _backgroundJobServices.CheckCronJobExists(delTokensSetting.JobName)
                };
                return Ok(delTokensJob);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
