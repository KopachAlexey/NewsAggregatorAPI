namespace NewsAggregatorData.Entities
{
    public class User
    {
        private Guid _id;

        public Guid Id => _id;
        public string PasswordHash { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public List<Comment> Comments { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<UserCommentReaction> UserCommentReactions { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}