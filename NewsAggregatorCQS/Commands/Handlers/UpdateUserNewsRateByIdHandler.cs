using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class UpdateUserNewsRateByIdHandler : IRequestHandler<UpdateUserNewsRateByIdCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public UpdateUserNewsRateByIdHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateUserNewsRateByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id.Equals(request.UserId),
                cancellationToken);
            if (user is null)
                return;
            user.NewsMinRate = request.NewNewsMinRate;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
