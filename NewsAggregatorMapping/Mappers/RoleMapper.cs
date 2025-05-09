using NewsAggregatorCore.DTO;
using NewsAggregatorData.Entities;
using Riok.Mapperly.Abstractions;

namespace NewsAggregatorMapping.Mappers
{
    [Mapper]
    public partial class RoleMapper
    {
        [MapperIgnoreTarget(nameof(Role.Id))]
        //[MapperIgnoreTarget(nameof(Role.Users))]
        public partial Role RoleDTOToEntity(RoleDTO roleDTO);

        public partial RoleDTO EntityToRoleDTO(Role role);
    }
}
