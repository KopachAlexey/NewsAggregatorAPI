using MediatR;
using NewsAggregatorCore.DTO;

namespace NewsAggregatorCQS.Querys
{
    public class GetAllRolesQuery : IRequest<RoleDTO[]>
    {
    }
}
