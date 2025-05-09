using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelRefreshTokenByIdHandler : IRequestHandler<DelRefreshTokenByIdCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelRefreshTokenByIdHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DelRefreshTokenByIdCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _dbContext.RefreshTokens
                .SingleOrDefaultAsync(t => t.Id.Equals(request.Id), cancellationToken);
            _dbContext.RefreshTokens.Remove(refreshToken);
            await _dbContext.SaveChangesAsync();
        }
    }
}
