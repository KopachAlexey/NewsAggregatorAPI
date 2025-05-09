using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetUserByIdQuery : IRequest<UserDTO?>
    {
        public Guid Id { get; init; }
    }
}
