using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelNewsByIdHandler : IRequestHandler<DelNewsByIdCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelNewsByIdHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DelNewsByIdCommand request, CancellationToken cancellationToken)
        {
            var news = await _dbContext.News
                .SingleOrDefaultAsync(n => n.Id.Equals(request.Id), cancellationToken);
            if (news is not null)
            {
                _dbContext.News.Remove(news);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
