using MediatR;

namespace NewsAggregatorCQS.Commands
{
    public class DelNewsByIdCommand : IRequest
    {
        public Guid Id { get; init; }
    }
}
