using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorCore;
using NewsAggregatorCore.DTO;
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
        readonly IValidator<UpdateUserRequest> _updateUserValidator;
        readonly ILogger<UserController> _logger;

        public UserController(IUserServices userServices, IPasswordHashing passwordHashing, UserMapper userMapper,
            IValidator<AddUserRequest> userValidator, OperationMapper operationMapper, 
            IValidator<UpdateUserRequest> updateUserValidator, ILogger<UserController> logger)
        {
            _userServices = userServices;
            _passwordHashing = passwordHashing;
            _userMapper = userMapper;
            _userValidator = userValidator;
            _operationMapper = operationMapper;
            _updateUserValidator = updateUserValidator;
            _updateUserValidator = updateUserValidator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest newUser)
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
                {
                    _logger.LogInformation($"Edd new user with login = {newUser.Login} " +
                        $"and id = {addUserResult.Id}");
                    return Created($"api/User/{addUserResult.Id}", new { Id = addUserResult.Id });
                }
                else
                    return Conflict(_operationMapper.ResultToResponse(addUserResult.OperationResult));
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to add new user with login = {newUser.Login}" +
                    $" and email = {newUser.Email}");
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
                _logger.LogError($"Error while trying to get user by login = {login}");
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
                _logger.LogError($"Error while trying to get user by user id = {id}");
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
                _logger.LogError($"Error while trying to del user by login = {login}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserById(Guid id, [FromBody]UpdateUserRequest newUserData)
        {
            try
            {
                var validationResult = _updateUserValidator.Validate(newUserData);
                if (!validationResult.IsValid)
                    return BadRequest();
                var user = await _userServices.GetUserByIdAsync(id);
                if (user is null)
                    return NotFound();
                if (!_passwordHashing.VerifyPassword(newUserData.Password, user.PasswordHash))
                    return Unauthorized();
                var userDTO = new UpdateUserDTO
                {
                    Login = user.Login,
                    Email = user.Email,
                    PasswordHash = newUserData.NewPassword is null ? user.PasswordHash 
                    : _passwordHashing.HashPassword(newUserData.NewPassword),
                    NewLogin = newUserData.Login,
                    NewEmail = newUserData.Email
                };
                var operationResult = await _userServices.UpdateUserByIdAsync(id, userDTO);
                if(operationResult.IsSuccessful)
                    return NoContent();
                else
                    return Conflict(_operationMapper.ResultToResponse(operationResult));
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to update user, user id = {id} " +
                    $"new loggin = {newUserData.Login} new email = {newUserData.Email}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPatch("update-user-news-rate/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserNewsRateById(Guid id, [FromBody] UpdateUserNewsRateRequest updateUserNewsRate)
        {
            try
            {
                if (updateUserNewsRate.NewsRate < NewsAggregatorConstants.MinNewsRate 
                        || updateUserNewsRate.NewsRate > NewsAggregatorConstants.MaxNewsRate)
                    return BadRequest();
                var user = await _userServices.GetUserByIdAsync(id);
                if (user is null)
                    return NotFound();
                await _userServices.UpdateUserNewsRateByIdAsync(id, updateUserNewsRate.NewsRate);
                return NoContent();
            }
            catch (Exception)
            {
                _logger.LogError($"Error while trying to update user news rate, user id = {id}" +
                    $"new news rate = {updateUserNewsRate.NewsRate}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
