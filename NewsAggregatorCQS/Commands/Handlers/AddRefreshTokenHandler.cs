using MediatR;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class AddRefreshTokenHandler : IRequestHandler<AddRefreshTokenCommand, Guid>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly TokenMapper _tokenMapper;

        public AddRefreshTokenHandler(NewsAggregatorContext dbContext, TokenMapper tokenMapper)
        {
            _dbContext = dbContext;
            _tokenMapper = tokenMapper;
        }

        public async Task<Guid> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var token = _tokenMapper.RefreshTokeDTOToEntity(request.RefreshTokenDTO);
            await _dbContext.RefreshTokens.AddAsync(token, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return token.Id;
        }
    }
}
