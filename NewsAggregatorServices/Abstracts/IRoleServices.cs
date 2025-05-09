using NewsAggregatorCore.DTO;

namespace NewsAggregatorServices.Abstracts
{
    public interface IRoleServices
    {
        public Task<RoleDTO[]> GetAllRolesAsync();

        public Task<RoleDTO?> GetRoleByName(string name);
    }
}
