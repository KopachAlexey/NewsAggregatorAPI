using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly UserMapper _userMapper;

        public GetUserByIdHandler(NewsAggregatorContext dbContext, UserMapper userMapper)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
        }

        public async Task<UserDTO?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken);
            return user is null ? null : _userMapper.EntityToUserDTO(user);
        }
    }
}
