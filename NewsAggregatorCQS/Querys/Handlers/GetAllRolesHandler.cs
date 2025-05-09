using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregatorCore.DTO;
using NewsAggregatorData;
using NewsAggregatorMapping.Mappers;

namespace NewsAggregatorCQS.Querys.Handlers
{
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, RoleDTO[]>
    {
        readonly NewsAggregatorContext _dbContext;
        readonly RoleMapper _roleMapper;

        public GetAllRolesHandler(NewsAggregatorContext dbContext, RoleMapper roleMapper)
        {
            _dbContext = dbContext;
            _roleMapper = roleMapper;
        }

        public async Task<RoleDTO[]> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _dbContext.Roles
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);
            if (roles is null || !roles.Any())
                return Array.Empty<RoleDTO>();
            return roles.Select(r => _roleMapper.EntityToRoleDTO(r)).ToArray();
        }
    }
}
