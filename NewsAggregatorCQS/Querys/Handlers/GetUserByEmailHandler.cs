using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly UserMapper _userMapper;

        public GetUserByEmailHandler(NewsAggregatorContext dbContext, UserMapper userMapper)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
        }

        public async Task<UserDTO?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            return user is null ? null : _userMapper.EntityToUserDTO(user);
        }
    }
}
