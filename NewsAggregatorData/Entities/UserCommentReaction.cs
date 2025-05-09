namespace NewsAggregatorData.Entities
{
    public class UserCommentReaction
    {
        private Guid _id;
        public Guid Id => _id;
        public Reaction? Reaction { get; set; }
        public Guid ReactionId { get; set; }
        public User? User { get; set; }
        public Guid UserId { get; set; }
        public Comment? Comment { get; set; }
        public Guid CommentId { get; set; }
    }
}
