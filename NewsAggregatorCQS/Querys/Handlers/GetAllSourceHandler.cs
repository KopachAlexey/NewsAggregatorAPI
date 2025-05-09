using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetAllSourceHandler : IRequestHandler<GetAllSourceQuery, SourceDTO[]>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly SourceMapper _sourceMapper = new();

        public GetAllSourceHandler(NewsAggregatorContext dbContext) => _dbContext = dbContext;

        public async Task<SourceDTO[]> Handle(GetAllSourceQuery request, CancellationToken cancellationToken)
        {
            var sources = await _dbContext.Sources
               .AsNoTracking()
               .ToArrayAsync();
            if (sources is null)
                return Array.Empty<SourceDTO>();
            var sourcesDTO = sources
                .Select(s => _sourceMapper.EntityToDto(s))
                .ToArray();
            return sourcesDTO;
        }
    }
}
