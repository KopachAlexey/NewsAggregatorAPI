using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class AddCommentCommand : IRequest<Guid>
    {
        public CommentDTO CommentDTO { get; init; }
    }
}
