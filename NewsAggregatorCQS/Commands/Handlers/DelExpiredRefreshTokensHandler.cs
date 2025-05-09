using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelExpiredRefreshTokensHandler : IRequestHandler<DelExpiredRefreshTokensCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelExpiredRefreshTokensHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DelExpiredRefreshTokensCommand request, CancellationToken cancellationToken)
        {
            var expiredTokens = await _dbContext.RefreshTokens
                .Where(t => t.RefreshTokenExpiryTime <= DateTime.UtcNow)
                .ToArrayAsync(cancellationToken);
            if (expiredTokens is not null && expiredTokens.Any())
            {
                _dbContext.RefreshTokens.RemoveRange(expiredTokens);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
