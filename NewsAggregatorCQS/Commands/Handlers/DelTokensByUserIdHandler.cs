using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelTokensByUserIdHandler : IRequestHandler<DelTokensByUserIdCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelTokensByUserIdHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DelTokensByUserIdCommand request, CancellationToken cancellationToken)
        {
            var tokens = await _dbContext.RefreshTokens
                .Where(t => t.UserId.Equals(request.UserId))
                .ToArrayAsync();
            if(tokens is not null && tokens.Any())
            {
                _dbContext.RemoveRange(tokens);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
