using MediatR;

namespace NewsAggregatorCQS.Commands
{
    public class DelTokensByUserIdCommand : IRequest
    {
        public Guid UserId { get; init; }
    }
}
