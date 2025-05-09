using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class AddUserReactionToCommentCommand : IRequest<Guid>
    {
        public UserCommentReactionDTO NewUserReactionToComment { get; set; }
    }
}
