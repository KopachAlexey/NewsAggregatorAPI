namespace NewsAggregatorCore
{
    public static class NewsAggregatorConstants
    {
        public const int RefreshTokenByteSize = 32;
        public const string RefreshTokenIdClaim = "RefreshTokenId";

        public const int MaxNewsRate = 5;
        public const int MinNewsRate = -5;
        public const int DefaultPageSize = 1;
        public const int DefaultPageNumber = 1;

        public const int MinLoginLenth = 3;
        public const int MaxLoginLenth = 20;
        public const int MinPasswordLenth = 10;
        public const int MaxPasswordLenth = 30;
        public const int MinEmailLenth = 5;

        public const string BeltaKey = "Belta";
        public const string TelegrafKey = "Telegraf";
        public const string RiaKey = "Ria";
        public const string NewsRaterKey = "NewsRater";
        public const string DelExpiredTokensKey = "DelExpiredTokens";

        public const string UserRole = "User";

        public const string LikeReactionKey = "Like";
        public const string DislikeReactionKey = "Dislike";

    }
}
