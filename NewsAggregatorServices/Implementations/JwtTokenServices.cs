using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsAggregatorCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Commands;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class JwtTokenServices : ITokenServices
    {
        readonly IConfiguration _configuration;
        readonly IMediator _mediator;

        public JwtTokenServices(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<AuthTokensDTO> GenerateTokensAsync(UserDTO user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
            var accessTokenExpiration = DateTime.UtcNow;
            if (Int32.TryParse(jwtSettings["AccessTokenExpirationMinutes"], out var accessTokenExpirationMinutes))
                accessTokenExpiration = accessTokenExpiration.AddMinutes(accessTokenExpirationMinutes);
            else
                throw new Exception("Parsing access token expiration faild");
            var refreshToken = await GenerateRefreshTokenAsync(user.Id);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Role, user.Role?.RoleName ?? String.Empty),
                    new Claim(NewsAggregatorConstants.RefreshTokenIdClaim, refreshToken.TokenID.ToString())
                }),
                NotBefore = DateTime.UtcNow,
                Expires = accessTokenExpiration,
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(secret), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var accessToken = jwtTokenHandler.CreateToken(tokenDescriptor);
            return new AuthTokensDTO
            {
                AccessToken = jwtTokenHandler.WriteToken(accessToken),
                RefreshToken = refreshToken.Token,
                AccessTokenExpiration = accessTokenExpiration
            };
        }

        private async Task<(Guid TokenID, string Token)> GenerateRefreshTokenAsync(Guid userId)
        {
            var randomNumber = new byte[NewsAggregatorConstants.RefreshTokenByteSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);
            var refreshTokenExpiration = DateTime.UtcNow;
            var jwtSettings = _configuration.GetSection("JwtSettings");
            if (Int32.TryParse(jwtSettings["RefreshTokenExpirationHours"], out var refreshTokenExpirationHours))
                refreshTokenExpiration = refreshTokenExpiration.AddHours(refreshTokenExpirationHours);
            else
                throw new Exception("Parsing refresh token expiration faild");
            var refreshTokenId = await _mediator.Send(new AddRefreshTokenCommand
            {
                RefreshTokenDTO = new RefreshTokenDTO
                {
                    Token = refreshToken,
                    UserId = userId,
                    RefreshTokenExpiryTime = refreshTokenExpiration,
                }
            });
            return (refreshTokenId, refreshToken);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secret = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public async Task DelRefreshTokenByIdAsync(Guid id)
        {
            await _mediator.Send(new DelRefreshTokenByIdCommand { Id = id });
        }

        public async Task<bool> VerifyRefreshTokenAsync(Guid tokenId, Guid userId, string token)
        {
            var refreshToken = await _mediator.Send(new GetRefreshTokenByIdQuery { Id =  tokenId });
            if(refreshToken is  null || !refreshToken.UserId.Equals(userId) || !refreshToken.Token.Equals(token) 
                || refreshToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return false;
            return true;
        }

        public async Task DelExpiredRefreshTokensAsync()
        {
            await _mediator.Send(new DelExpiredRefreshTokensCommand());
        }
    }
}
