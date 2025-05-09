using MediatR;

namespace NewsAggregatorCQS.Commands
{
    public class DelRefreshTokenByIdCommand : IRequest
    {
        public Guid Id { get; init; }
    }
}
