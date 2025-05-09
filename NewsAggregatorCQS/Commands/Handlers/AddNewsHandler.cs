using MediatR;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class AddNewsHandler : IRequestHandler<AddNewsCommands>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly NewsMapper _newsMapper;

        public AddNewsHandler(NewsAggregatorContext dbContext, NewsMapper newsMapper)
        {
            _dbContext = dbContext;
            _newsMapper = newsMapper;
        }

        public async Task Handle(AddNewsCommands request, CancellationToken cancellationToken)
        {
            var newNews = request.NewsDTOs.Select(n => _newsMapper.NewsDtoToEntity(n));
            await _dbContext.News.AddRangeAsync(newNews, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
