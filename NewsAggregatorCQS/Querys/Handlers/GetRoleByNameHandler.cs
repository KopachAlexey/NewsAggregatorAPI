using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetRoleByNameHandler : IRequestHandler<GetRoleByNameQuery, RoleDTO?>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly RoleMapper _roleMapper;

        public GetRoleByNameHandler(NewsAggregatorContext dbContext, RoleMapper roleMapper)
        {
            _dbContext = dbContext;
            _roleMapper = roleMapper;
        }

        public async Task<RoleDTO?> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles
                .SingleOrDefaultAsync(r => r.RoleName == request.RoleName, cancellationToken);
            return role is null ? null : _roleMapper.EntityToRoleDTO(role);
        }
    }
}
