
namespace NewsAggregatorModels.Models
{
    public class AddCommentRequest
    {
        public string Text { get; init; }
        public DateTimeOffset CreationDate { get; init; }
        public Guid UserId { get; init; }
        public Guid NewsId { get; init; }
    }
}
