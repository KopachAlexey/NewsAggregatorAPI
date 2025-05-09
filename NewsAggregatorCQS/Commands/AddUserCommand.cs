using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class AddUserCommand : IRequest<Guid>
    {
        public UserDTO NewUser { get; init; }
    }
}
