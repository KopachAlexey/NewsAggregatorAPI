using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class UpdateUserReactionToCommentCommand : IRequest
    {
        public UserCommentReactionDTO UserReactionToComment { get; set; }
    }
}
