using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class DelCommentByIdHandler : IRequestHandler<DelCommentByIdCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public DelCommentByIdHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(DelCommentByIdCommand request, CancellationToken cancellationToken)
        {
            var comment = await _dbContext.Comments.SingleOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);
            if(comment is not null)
            {
                _dbContext.Comments.Remove(comment);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
