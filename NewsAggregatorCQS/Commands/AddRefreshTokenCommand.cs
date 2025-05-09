using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Commands
{
    public class AddRefreshTokenCommand : IRequest<Guid>
    {
        public RefreshTokenDTO RefreshTokenDTO { get; init; }
    }
}
