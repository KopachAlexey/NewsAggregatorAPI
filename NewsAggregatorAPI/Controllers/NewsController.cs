using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorCore.DTO;
using NewsAggregatorMapping.Mappers;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        readonly INewsServices _newsServices;
        readonly IValidator<GetNewsPageRequest> _newsPageValidator;
        readonly OperationMapper _operationMapper;
        readonly ILogger<NewsController> _logger;

        public NewsController(INewsServices newsServices, IValidator<GetNewsPageRequest> newsPageValidator, 
            OperationMapper operationMapper, ILogger<NewsController> logger)
        {
            _newsServices = newsServices;
            _newsPageValidator = newsPageValidator;
            _operationMapper = operationMapper;
            _logger = logger;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NewsDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNewsById(Guid id)
        {
            try
            {
                var news = await _newsServices.GetByIdAsync(id);
                return news is null ? BadRequest() : Ok(news);
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to get news with id = {id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Developer, Admin, Moderator")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNewsRateById(Guid id, double newRate)
        {
            try
            {
                var validationResult = _newsPageValidator.Validate(new GetNewsPageRequest { MinRate = newRate }, opt =>
                {
                    opt.IncludeProperties(n => n.MinRate);
                });
                if (!validationResult.IsValid)
                    return BadRequest();
                var operationResult = await _newsServices.UpdateNewsRateByIdAsync(id, newRate);
                if (operationResult.IsSuccessful)
                    return NoContent();
                else
                    return Conflict(_operationMapper.ResultToResponse(operationResult));
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to update news rate, news id = {id} " +
                    $"new rate = {newRate}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
