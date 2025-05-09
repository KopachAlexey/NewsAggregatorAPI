using MediatR;

namespace NewsAggregatorCQS.Querys
{
    public class CheckUserExistenceQuery : IRequest<bool>
    {
        public string Login { get; init; }
        public string Email { get; init; }
    }
}
