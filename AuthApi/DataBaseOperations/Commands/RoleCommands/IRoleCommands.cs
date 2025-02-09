using AuthApi.Dto;

namespace AuthApi.DataBaseOperations.Commands.RoleCommands
{
    public interface IRoleCommands
    {
        public Task<IResult> AddNewRole(string roleName);
        public Task<IResult> UpdateRole(RoleDto roleDto);
        public Task<IResult> DeleteRole(string roleId);
    }
}
