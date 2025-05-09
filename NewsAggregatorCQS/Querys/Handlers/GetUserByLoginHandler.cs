using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;
using NewsAggregatorCore.DTO;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetUserByLoginHandler : IRequestHandler<GetUserByLoginQuery, UserDTO?>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly UserMapper _userMapper;

        public GetUserByLoginHandler(NewsAggregatorContext dbContext, UserMapper userMapper)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
        }

        public async Task<UserDTO?> Handle(GetUserByLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Login == request.Login, cancellationToken);
            return user is null? null : _userMapper.EntityToUserDTO(user);
        }
    }
}
