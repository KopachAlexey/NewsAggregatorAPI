using NewsAggregatorCore.DTO;
using MediatR;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class SourceServices : ISourceServices
    {
        readonly IMediator _mediator;

        public SourceServices(IMediator mediator) => _mediator = mediator;

        public async Task<SourceDTO[]> GetAllAsync()
        {
            return await _mediator.Send(new GetAllSourceQuery());
        }
    }
}
