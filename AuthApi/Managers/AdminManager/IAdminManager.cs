using AuthApi.Dto;

namespace AuthApi.Managers.AdminManager
{
    public interface IAdminManager
    {
        public Task<IResult> RegisterNewUser(UserAdminRegistrationDto userDto, CancellationToken cancellationToken);
        public Task<IResult> ChangeUserPassword(UserAdminPasswordChangeDto userPasswordChangeDto, CancellationToken cancellationToken);
        public Task<IResult> UpdateUserAccount(UserAdminAccountUpdateDto userDto, CancellationToken cancellationToken);
        public Task<IResult> DeleteUser(string userId, CancellationToken cancellationToken);
        public Task<IResult> GetAllUsers(CancellationToken cancellationToken);
        public Task<IResult> GetUser(string userId, CancellationToken cancellationToken);
        public Task<IResult> AddNewRole(string roleName, CancellationToken cancellationToken);
        public Task<IResult> UpdateRole(RoleDto roleDto, CancellationToken cancellationToken);
        public Task<IResult> DeleteRole(string roleId, CancellationToken cancellationToken);
        public Task<IResult> GetRoles(CancellationToken cancellationToken);
        public Task<IResult> GetRoleById(string roleId, CancellationToken cancellationToken);
    }
}
