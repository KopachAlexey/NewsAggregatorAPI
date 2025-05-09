
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelAllNewsHandler : IRequestHandler<DelAllNewsCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelAllNewsHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;
        public async Task Handle(DelAllNewsCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.News.ExecuteDeleteAsync();
        }
    }
}
