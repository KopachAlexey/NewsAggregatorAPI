namespace NewsAggregatorCore.DTO
{
    public class RefreshTokenDTO
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset RefreshTokenExpiryTime { get; set; }
        public Guid UserId { get; set; }
    }
}
