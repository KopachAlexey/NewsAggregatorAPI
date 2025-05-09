namespace NewsAggregatorModels.Models
{
    public class UpdateTokensRequest
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
