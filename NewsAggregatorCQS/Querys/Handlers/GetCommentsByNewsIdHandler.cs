using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetCommentsByNewsIdHandler : IRequestHandler<GetCommentsByNewsIdQuery, CommentDTO[]>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly CommentMapper _comentMapper;

        public GetCommentsByNewsIdHandler(NewsAggregatorContext dbContext, CommentMapper comentMapper)
        {
            _dbContext = dbContext;
            _comentMapper = comentMapper;
        }

        public async Task<CommentDTO[]> Handle(GetCommentsByNewsIdQuery request, CancellationToken cancellationToken)
        {
            var comments = await _dbContext.Comments
                .AsNoTracking()
                .Include(c => c.News)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.UserCommentReactions)
                    .ThenInclude(u => u.User)
                .Include(c => c.UserCommentReactions)
                    .ThenInclude(u => u.Reaction)
                .Where(c => c.NewsId.Equals(request.NewsId))
                .ToArrayAsync(cancellationToken);
            if (comments is null || !comments.Any())
                return Array.Empty<CommentDTO>();
            return comments.Select(c => _comentMapper.EntityToCommentDTO(c)).ToArray();
        }
    }
}
