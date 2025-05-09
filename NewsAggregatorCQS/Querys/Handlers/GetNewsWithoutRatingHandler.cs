
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorCore.DTO;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetNewsWithoutRatingHandler : IRequestHandler<GetNewsWithoutRatingQuery, NewsDTO[]>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly NewsMapper _newsMapper = new();

        public GetNewsWithoutRatingHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;
        

        public async Task<NewsDTO[]> Handle(GetNewsWithoutRatingQuery request, CancellationToken cancellationToken)
        {
            var newsWithoutRating = await _dbContext.News
                .AsNoTracking()
                .Where(n => !n.PositivityRate.HasValue)
                .ToArrayAsync(cancellationToken);
            if (newsWithoutRating is null)
                return Array.Empty<NewsDTO>();
            return newsWithoutRating.Select(n => _newsMapper.EntityToNewsDto(n)).ToArray();
        }
    }
}
