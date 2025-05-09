using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorData.Entities;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class UpdateNewsRatingHandler : IRequestHandler<UpdateNewsRatingCommand>
    {
        private readonly NewsAggregatorContext _dbContext;

        public UpdateNewsRatingHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;


        public async Task Handle(UpdateNewsRatingCommand request, CancellationToken cancellationToken)
        {
            var updatedNews = await _dbContext.News
              .Where(n => request.RatingById.Keys.Contains(n.Id))
              .ToArrayAsync(cancellationToken);
            if (updatedNews is null)
                return;
            foreach (var news in updatedNews)
                news.PositivityRate = request.RatingById.GetValueOrDefault(news.Id);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
