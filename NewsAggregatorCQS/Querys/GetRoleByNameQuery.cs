using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetRoleByNameQuery : IRequest<RoleDTO?>
    {
        public string RoleName { get; init; }
    }
}
