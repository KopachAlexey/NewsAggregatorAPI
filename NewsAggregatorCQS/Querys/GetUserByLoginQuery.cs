using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetUserByLoginQuery : IRequest<UserDTO?>
    {
        public string Login { get; init; }
    }
}
