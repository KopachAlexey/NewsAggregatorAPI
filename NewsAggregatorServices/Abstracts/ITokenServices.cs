using System.Security.Claims;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface ITokenServices
    {
        Task<AuthTokensDTO> GenerateTokensAsync(UserDTO user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
        Task<bool> VerifyRefreshTokenAsync(Guid tokenId, Guid userId, string token);
        Task DelRefreshTokenByIdAsync(Guid id);
        Task DelExpiredRefreshTokensAsync();
    }
}
