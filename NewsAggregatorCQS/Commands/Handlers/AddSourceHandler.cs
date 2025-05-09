using MediatR;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class AddSourceHandler : IRequestHandler<AddSourceCommand>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly SourceMapper _sourceMapper = new();

        public AddSourceHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;

        public async Task Handle(AddSourceCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Sources.AddAsync(_sourceMapper.DtoToEntity(request.SourceDTO));
            await _dbContext.SaveChangesAsync();
        }
    }
}
