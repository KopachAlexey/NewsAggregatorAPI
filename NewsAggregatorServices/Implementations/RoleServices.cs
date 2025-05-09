using MediatR;
using NewsAggregatorCore.DTO;
using NewsAggregatorCQS.Querys;
using NewsAggregatorServices.Abstracts;

namespace NewsAggregatorServices.Implementations
{
    public class RoleServices : IRoleServices
    {
        readonly IMediator _mediator;

        public RoleServices(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RoleDTO[]> GetAllRolesAsync()
        {
            return await _mediator.Send(new GetAllRolesQuery());
        }

        public async Task<RoleDTO?> GetRoleByName(string name)
        {
            return await _mediator.Send(new GetRoleByNameQuery { RoleName = name });
        }
    }
}
