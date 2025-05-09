using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class UpdateUserByIdCommand : IRequest
    {
        public Guid Id { get; init; }
        public UserDTO NewUserData { get; init; }
    }
}
