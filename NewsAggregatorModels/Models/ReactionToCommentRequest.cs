namespace NewsAggregatorModels.Models
{
    public class ReactionToCommentRequest
    {
        public string ReactionName { get; set; }
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }
    }
}
