using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelAllSourceHandler : IRequestHandler<DelAllSourceCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelAllSourceHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;
        public async Task Handle(DelAllSourceCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Sources.ExecuteDeleteAsync();
        }
    }
}
