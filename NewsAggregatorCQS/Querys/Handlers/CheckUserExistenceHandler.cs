using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class CheckUserExistenceHandler : IRequestHandler<CheckUserExistenceQuery, bool>
    {
        private NewsAggregatorContext _dbContext;

        public CheckUserExistenceHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CheckUserExistenceQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .SingleOrDefaultAsync(u => u.Email == request.Email || u.Login == request.Login, cancellationToken);
            return user is null ? false : true;
        }
    }
}
