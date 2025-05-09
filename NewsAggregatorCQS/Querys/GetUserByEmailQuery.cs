using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetUserByEmailQuery : IRequest<UserDTO?>
    {
        public string Email { get; init; }
    }
}
