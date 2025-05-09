using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelUserByLoginHandler : IRequestHandler<DelUserByLoginCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelUserByLoginHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DelUserByLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Login == request.Login, cancellationToken);
            if(user is not null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
