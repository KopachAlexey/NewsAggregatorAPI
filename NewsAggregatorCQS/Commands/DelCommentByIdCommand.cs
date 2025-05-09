using MediatR;

namespace NewsAggregatorCQS.Commands
{
    public class DelCommentByIdCommand : IRequest
    {
        public Guid Id { get; init;}
    }
}
