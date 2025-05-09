using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorCore.DTO;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetNewsCardByRateHandler : IRequestHandler<GetNewsCardsByRateQuery, NewsCardDTO[]>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly NewsMapper _newsMapper;

        public GetNewsCardByRateHandler(NewsAggregatorContext dbContext, NewsMapper newsMapper)
        {
            _dbContext = dbContext;
            _newsMapper = newsMapper;
        }

        public async Task<NewsCardDTO[]> Handle(GetNewsCardsByRateQuery request, CancellationToken cancellationToken)
        {
            var positiveNews = await _dbContext.News
               .AsNoTracking()
               .Include(n => n.Source)
               .Where(n => n.PositivityRate.HasValue && n.PositivityRate >= request.MinRate)
               .OrderByDescending(n => n.PublicationDate)
               .Skip(request.SkipNewsCount)
               .Take(request.TakeNewsCount)
               .ToArrayAsync();
            if (positiveNews is null)
                return Array.Empty<NewsCardDTO>();
            var positiveNewsDTO = positiveNews
                .Select(pn => _newsMapper.EntityToNewsCardDto(pn))
                .ToArray();
            return positiveNewsDTO;
        }
    }
}
