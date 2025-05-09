using Microsoft.AspNetCore.Mvc;
using NewsAggregatorMapping.Mappers;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        readonly ISourceServices _sourceServices;
        readonly SourceMapper _sourceMapper;

        public SourceController(ISourceServices sourceServices, SourceMapper sourceMapper)
        {
            _sourceServices = sourceServices;
            _sourceMapper = sourceMapper;
        }
   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SourceResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<SourceResponse>>> GetAllSource()
        {
            try 
            {
                var sources = await _sourceServices.GetAllAsync();
                var sourceModels = sources.Select(s => _sourceMapper.DtoToModel(s)).ToList();
                return Ok(sourceModels);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }
    }
}
