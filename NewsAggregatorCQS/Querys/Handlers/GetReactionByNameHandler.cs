using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetReactionByNameHandler : IRequestHandler<GetReactionByNameQuery, ReactionDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly ReactionMapper _reactionMapper;

        public GetReactionByNameHandler(NewsAggregatorContext dbContext, ReactionMapper reactionMapper)
        {
            _dbContext = dbContext;
            _reactionMapper = reactionMapper;
        }

        public async Task<ReactionDTO?> Handle(GetReactionByNameQuery request, CancellationToken cancellationToken)
        {
            var reaction = await _dbContext.Reactions.SingleOrDefaultAsync(r => r.Name == request.ReactionName);
            if (reaction is null)
                return null;
            return _reactionMapper.EntityToDto(reaction);
        }
    }
}
