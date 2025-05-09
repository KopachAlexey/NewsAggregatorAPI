namespace NewsAggregatorCore
{
    public static class NewsAggregatorMessages
    {
        public const string LoginFailed = "Incorrect login or password";
        public const string UniqueUserLogin = "User with this login already exists";
        public const string UniqueUserEmail = "User with this email already exists";
        public const string UserDoesNotExist = "User with such login does not exist";
        public const string UserRoleNotExist = "Role with name 'User' does not exist";

        public const string CommentDoesNotExist = "Comment does not exist";

        public const string NoSuchReaction = "There is no reaction with this name";
        public const string CommentReactionAlredyExists = "There is already a user reaction to this comment";
        public const string CommentReactionDontExists = "There is no user reaction to this comment";

        public readonly static string IncorrectRate = $"News rating cannot be more than {NewsAggregatorConstants.MaxNewsRate} " +
            $"and less than {NewsAggregatorConstants.MinNewsRate}";
    }
}
