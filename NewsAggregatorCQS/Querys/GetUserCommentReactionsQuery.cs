using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetUserCommentReactionsQuery : IRequest<UserCommentReactionDTO[]>
    {
        public Guid CommentId { get; init; }
        public Guid ReactionId { get; init; }
    }
}
