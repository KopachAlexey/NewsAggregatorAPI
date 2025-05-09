using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class UpdateNewsRateByIdHandler : IRequestHandler<UpdateNewsRateByIdCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public UpdateNewsRateByIdHandler(NewsAggregatorContext dbContext, NewsMapper newsMapper)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateNewsRateByIdCommand request, CancellationToken cancellationToken)
        {
            var news = await _dbContext.News.SingleOrDefaultAsync(n => n.Id.Equals(request.Id), cancellationToken);
            if(news is not null)
            {
                news.PositivityRate = request.NewRate;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            
        }
    }
}
