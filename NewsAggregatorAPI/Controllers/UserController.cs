using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorMapping.Mappers;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserServices _userServices;
        readonly IPasswordHashing _passwordHashing;
        readonly UserMapper _userMapper;
        readonly OperationMapper _operationMapper;
        readonly IValidator<AddUserRequest> _userValidator;

        public UserController(IUserServices userServices, IPasswordHashing passwordHashing, UserMapper userMapper,
            IValidator<AddUserRequest> userValidator, OperationMapper operationMapper)
        {
            _userServices = userServices;
            _passwordHashing = passwordHashing;
            _userMapper = userMapper;
            _userValidator = userValidator;
            _operationMapper = operationMapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser([FromForm] AddUserRequest newUser)
        {
            try
            {
                var validationResult = _userValidator.Validate(newUser);
                if (!validationResult.IsValid)
                    return BadRequest();
                var newUserDTO = _userMapper.NewUserReuestToUserDTO(newUser);
                newUserDTO.PasswordHash = _passwordHashing.HashPassword(newUser.Password);
                var addUserResult = await _userServices.AddNewUserAsync(newUserDTO);
                if (addUserResult.OperationResult.IsSuccessful)
                    return Created($"api/User/{addUserResult.Id}", new {Id = addUserResult.Id });
                else
                    return Conflict(_operationMapper.ResultToResponse(addUserResult.OperationResult));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("by-login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type =typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByLogin([FromQuery] string login)
        {
            try
            {
                var validationResult = _userValidator.Validate(new AddUserRequest { Login = login }, opt =>
                {
                    opt.IncludeProperties(u => u.Login);
                });
                if (!validationResult.IsValid)
                    return BadRequest();
                var user = await _userServices.GetUserByLoginAsync(login);
                if (user is null)
                    return NotFound();
                return Ok(_userMapper.UserDTOToUserResponse(user));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userServices.GetUserByIdAsync(id);
                if (user is null)
                    return NotFound();
                return Ok(_userMapper.UserDTOToUserResponse(user));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DelUserByLogin( [FromQuery] string login)
        {
            try
            {
                var validationResult = _userValidator.Validate(new AddUserRequest { Login = login }, opt =>
                {
                    opt.IncludeProperties(u => u.Login);
                });
                if (!validationResult.IsValid)
                    return BadRequest();
                var operationResult = await _userServices.DelUserByLoginAsync(login);
                if (operationResult.IsSuccessful)
                    return NoContent();
                else
                    return Conflict(_operationMapper.ResultToResponse(operationResult));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserById(Guid id, [FromForm]AddUserRequest newUserData)
        {
            try
            {
                var validationResult = _userValidator.Validate(newUserData);
                if (!validationResult.IsValid)
                    return BadRequest();
                var userDTO = _userMapper.NewUserReuestToUserDTO(newUserData);
                userDTO.PasswordHash = _passwordHashing.HashPassword(newUserData.Password);
                var operationResult = await _userServices.UpdateUserByIdAsync(id, userDTO);
                if(operationResult.IsSuccessful)
                    return NoContent();
                else
                    return Conflict(_operationMapper.ResultToResponse(operationResult));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
