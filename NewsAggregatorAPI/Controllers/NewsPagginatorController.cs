using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorCore.DTO;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsPagginatorController : ControllerBase
    {
        readonly INewsPagginator _newsPagginator;
        readonly IValidator<GetNewsPageRequest> _newsPageValidator;

        public NewsPagginatorController(INewsPagginator newsPagginator, IValidator<GetNewsPageRequest> newsPageValidator)
        {
            _newsPagginator = newsPagginator;
            _newsPageValidator = newsPageValidator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsPageDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNewsPage([FromQuery]GetNewsPageRequest newsPageRequest)
        {
            try
            {
                var validationResult = _newsPageValidator.Validate(newsPageRequest);
                if (!validationResult.IsValid)
                    return BadRequest();
                var newsPage = await _newsPagginator.GetNewsPageAsync(newsPageRequest.MinRate, newsPageRequest.PageNumber,
                    newsPageRequest.PageSize);
                return Ok(newsPage);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
