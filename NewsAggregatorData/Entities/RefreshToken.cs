namespace NewsAggregatorData.Entities
{
    public class RefreshToken
    {
        private Guid _id;
        public Guid Id => _id;
        public string Token { get; set; }
        public DateTimeOffset RefreshTokenExpiryTime { get; set; }
        public bool IsExpired { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
