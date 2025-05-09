using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetUserCommentReactionQuery : IRequest<UserCommentReactionDTO?>
    {
        public Guid UserId { get; init; }
        public Guid CommentId { get; init; }
    }
}
