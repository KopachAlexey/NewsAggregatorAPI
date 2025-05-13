using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorCore.DTO;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorAPI.Controllers
{
    [Authorize(Roles = "Developer, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        readonly IRoleServices _roleServices;
        readonly ILogger<RolesController> _logger;

        public RolesController(IRoleServices roleServices, ILogger<RolesController> logger)
        {
            _roleServices = roleServices;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoleDTO>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleServices.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception)
            {
                _logger.LogError("Error while trying to get all roles");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
