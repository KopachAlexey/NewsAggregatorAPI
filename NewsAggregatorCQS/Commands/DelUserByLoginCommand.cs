using MediatR;

namespace NewsAggregatorCQS.Commands
{
    public class DelUserByLoginCommand : IRequest
    {
        public string Login { get; init; }
    }
}
