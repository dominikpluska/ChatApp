using AuthApi.Dto;

namespace AuthApi.Managers.AdminManager
{
    public interface IAdminManager
    {
        public Task<IResult> RegisterNewUser(UserAdminRegistrationDto userDto);
        public Task<IResult> ChangeUserPassword(UserAdminPasswordChangeDto userPasswordChangeDto);
        public Task<IResult> UpdateUserAccount(UserAdminAccountUpdateDto userDto);
        public Task<IResult> DeleteUser(string userId);
        public Task<IResult> GetAllUsers();
        public Task<IResult> GetUser(string userId);
        public Task<IResult> AddNewRole(string roleName);
        public Task<IResult> UpdateRole(RoleDto roleDto);
        public Task<IResult> DeleteRole(string roleId);
        public Task<IResult> GetRoles();
        public Task<IResult> GetRoleById(string roleId);
    }
}
