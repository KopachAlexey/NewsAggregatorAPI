using NewsAggregatorCore.DTO;

namespace NewsAggregatorModels.Models
{
    public class CommentResponse
    {
        public Guid Id { get; init; }
        public string Text { get; set; }
        public DateTimeOffset CreationDate { get ; set; }
        public Guid UserId { get; init; }
        public string UserLogin { get; set; }
        public string UserRoleName { get; set; }
        public List<UserCommentReactionDTO> UserCommentReactions { get; set; } = new();
    }
}
