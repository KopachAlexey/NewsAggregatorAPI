using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorCore.DTO;


namespace NewsAggregatorCQS.Querys.Handlers
{
    public class FilterUniqueNewsHandler : IRequestHandler<FilterUniqueNewsQuery, NewsDTO[]>
    {
        readonly NewsAggregatorContext _dbContext;

        public FilterUniqueNewsHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;

        public async Task<NewsDTO[]> Handle(FilterUniqueNewsQuery request, CancellationToken cancellationToken)
        {
            var uniqueNews = new List<NewsDTO>();
            foreach (var n in request.NewsDTOs)
                if (await _dbContext.News.FirstOrDefaultAsync(dbn => dbn.Url.Equals(n.Url)) is null)
                    uniqueNews.Add(n);
            return uniqueNews.ToArray();
        }
    }
}
