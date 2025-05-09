
using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetRefreshTokenByIdQuery : IRequest<RefreshTokenDTO?>
    {
        public Guid Id { get; init; }
    }
}
