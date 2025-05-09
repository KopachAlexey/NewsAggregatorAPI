using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetRefreshTokenByIdHandler : IRequestHandler<GetRefreshTokenByIdQuery, RefreshTokenDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly TokenMapper _tokenMapper;

        public GetRefreshTokenByIdHandler(NewsAggregatorContext dbContext, TokenMapper tokenMapper)
        {
            _dbContext = dbContext;
            _tokenMapper = tokenMapper;
        }

        public async Task<RefreshTokenDTO?> Handle(GetRefreshTokenByIdQuery request, CancellationToken cancellationToken)
        {
            var token = await _dbContext.RefreshTokens
                .SingleOrDefaultAsync(t => t.Id.Equals(request.Id), cancellationToken);
            return token is null ? null : _tokenMapper.EntityToRefreshTokeDTO(token);
        }
    }
}
