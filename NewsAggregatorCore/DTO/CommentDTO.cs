namespace NewsAggregatorCore.DTO
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public UserDTO? User { get; set; }
        public Guid UserId { get; set; }
        public NewsDTO? News { get; set; }
        public Guid NewsId { get; set; }
        public List<UserCommentReactionDTO> UserCommentReactions { get; set; } = new();
    }
}
