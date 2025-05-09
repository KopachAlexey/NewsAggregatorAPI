using System.Security.Cryptography;

namespace NewsAggregatorCore.DTO
{
    public class UserCommentReactionDTO
    {
        public Guid Id { get; set; }
        public Guid ReactionId { get; set; }
        public string? ReactionName { get; set; }
        public Guid UserId { get; set; }
        public string? UserLogin { get; set; }
        public Guid CommentId { get; set; }
    }
}
