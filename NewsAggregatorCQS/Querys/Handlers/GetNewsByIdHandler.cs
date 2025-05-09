using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorCore.DTO;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetNewsByIdHandler : IRequestHandler<GetNewsByIdQuery, NewsDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly NewsMapper _newsMapper;

        public GetNewsByIdHandler(NewsAggregatorContext dbContext, NewsMapper newsMapper)
        {
            _dbContext = dbContext;
            _newsMapper = newsMapper;
        }

        public async Task<NewsDTO?> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            var news = await _dbContext.News
                .AsNoTracking()
                .Include(n => n.Source)
                .Include(n => n.Comments)
                .FirstOrDefaultAsync(n => n.Id.Equals(request.Id));
            return news is null? null : _newsMapper.EntityToNewsDto(news);
        }
    }
}
