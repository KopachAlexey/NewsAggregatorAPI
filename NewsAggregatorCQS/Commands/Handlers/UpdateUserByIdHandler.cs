using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorData;


namespace NewsAggregatorCQS.Commands.Handlers
{
    public class UpdateUserByIdHandler : IRequestHandler<UpdateUserByIdCommand>
    {
        readonly NewsAggregatorContext _dbContext;

        public UpdateUserByIdHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateUserByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id.Equals(request.Id));
            user.Login = request.NewUserData.Login;
            user.PasswordHash = request.NewUserData.PasswordHash;
            user.Email = request.NewUserData.Email;
            //user.RoleId = request.NewUserData.RoleId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
