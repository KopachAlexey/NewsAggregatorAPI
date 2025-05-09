using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class UpdateNewsRateByIdCommand : IRequest
    {
        public double NewRate { get; init; }
        public Guid Id { get; init; }
    }
}
