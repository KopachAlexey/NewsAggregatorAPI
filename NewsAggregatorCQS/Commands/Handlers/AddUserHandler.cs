using MediatR;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Commands.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, Guid>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly UserMapper _userMapper;

        public AddUserHandler(NewsAggregatorContext dbContext, UserMapper userMapper)
        {
            _dbContext = dbContext;
            _userMapper = userMapper;
        }

        public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var user = _userMapper.UserDTOToEntity(request.NewUser);
            await _dbContext.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
    }
}
