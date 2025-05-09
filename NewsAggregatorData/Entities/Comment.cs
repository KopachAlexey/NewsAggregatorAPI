namespace NewsAggregatorData.Entities
{
    public class Comment
    {
        private Guid _id;

        public Guid Id => _id;
        public string Text { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public User? User { get; set; }
        public Guid UserId { get; set; }
        public News? News { get; set; }
        public Guid NewsId { get; set; }
        public List<UserCommentReaction> UserCommentReactions { get; set; }
    }
}