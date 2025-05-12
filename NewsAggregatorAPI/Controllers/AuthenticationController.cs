using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorCore.DTO;
using NewsAggregatorModels.Models;
using NewsAggregatorServices.Abstracts;
using NewsAggregatorCore;
using FluentValidation;

namespace NewsAggregatorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        readonly ITokenServices _tokenServices;
        readonly IUserServices _userServices;
        readonly IPasswordHashing _passwordHashing;
        readonly IValidator<UpdateTokensRequest> _tokensValidator;
        readonly IValidator<AddUserRequest> _userValidator;

        public AuthenticationController(ITokenServices tokenServices, IUserServices userServices, 
            IPasswordHashing passwordHashing, IValidator<UpdateTokensRequest> tokensValidator, 
            IValidator<AddUserRequest> userValidator)
        {
            _tokenServices = tokenServices;
            _userServices = userServices;
            _passwordHashing = passwordHashing;
            _tokensValidator = tokensValidator;
            _userValidator = userValidator;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthTokensDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login( [FromForm]LoginRequest loginRequest)
        {
            try
            {
                var validationResult = _userValidator.Validate(
                new AddUserRequest
                {
                    Login = loginRequest.Login,
                    Password = loginRequest.Password
                },
                opt =>
                {
                    opt.IncludeProperties(u => u.Login);
                    opt.IncludeProperties(u => u.Password);
                });
                 if (!validationResult.IsValid)
                return BadRequest();
                var user = await _userServices.GetUserByLoginAsync(loginRequest.Login);
                if (user is null || !_passwordHashing.VerifyPassword(loginRequest.Password, user.PasswordHash))
                    return Unauthorized();
                else
                    return Ok(await _tokenServices.GenerateTokensAsync(user));
                }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (!Guid.TryParse(User.FindFirstValue(NewsAggregatorConstants.RefreshTokenIdClaim), out var refreshTokenId))
                    return Unauthorized();
                await _tokenServices.DelRefreshTokenByIdAsync(refreshTokenId);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
           
        }

        [HttpPost("update-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthTokensDTO))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateTokens( [FromBody] UpdateTokensRequest updateTokenRequest)
        {
            try
            {
                var validationResult = _tokensValidator.Validate(updateTokenRequest);
                if (!validationResult.IsValid)
                    return BadRequest();
                var principal = _tokenServices.GetPrincipalFromExpiredToken(updateTokenRequest.AccessToken);
                if (!Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
                    return Unauthorized();
                if (!Guid.TryParse(principal.FindFirstValue(NewsAggregatorConstants.RefreshTokenIdClaim), out var refreshTokenId))
                    return Unauthorized();
                var user = await _userServices.GetUserByIdAsync(userId);
                if (!await _tokenServices.VerifyRefreshTokenAsync(refreshTokenId, userId, updateTokenRequest.RefreshToken) 
                    || user is null)
                    return Unauthorized();
                await _tokenServices.DelRefreshTokenByIdAsync(refreshTokenId);
                return Ok(await _tokenServices.GenerateTokensAsync(user));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }   
    }
}
