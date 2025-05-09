
namespace NewsAggregatorCore.DTO
{
    public class AuthTokensDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset AccessTokenExpiration { get; set; }
        //public DateTime RefreshTokenExpiration { get; set; }
    }
}
