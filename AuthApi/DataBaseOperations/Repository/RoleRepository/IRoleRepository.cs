using AuthApi.Dto;

namespace AuthApi.DataBaseOperations.Repository.RoleRepository
{
    public interface IRoleRepository
    {
        public Task<IEnumerable<RoleDto>> GetRoles();
        public Task<RoleDto> GetRoleById(string roleId);
        public Task<RoleDto> GetRoleByName(string roleName);
    }
}
