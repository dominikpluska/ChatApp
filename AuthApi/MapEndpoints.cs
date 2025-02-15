using AuthApi.Dto;
using AuthApi.Managers.AdminManager;
using AuthApi.Managers.UserManager;
using System.Runtime.CompilerServices;

namespace AuthApi
{
    public static class MapEndpoints
    {
        public static WebApplication MapUserEndpoints(this WebApplication app, IUserManager userManager)
        {
            app.MapPost("/Register", async (UserRegistrationDto userDto) => await userManager.RegisterNewUser(userDto));
            app.MapPost("/Login", async (UserLoginDto loginDto) => await userManager.Login(loginDto));
            app.MapPost("/Logout", async () => await userManager.LogOut());
            app.MapPost("/ChangePassword", async (UserPasswordChangeDto passwordChange) => await userManager.UpdatePassword(passwordChange));
            app.MapPost("/UpdateUser", async (UserAccountUpdateDto userDto) => await userManager.UpdateAccount(userDto));
            app.MapGet("/AuthCheck", async () => await userManager.CheckAuthentication());
            app.MapGet("/GetAccountProperties", async (string userId) => await userManager.GetAccountProperties(userId));
            app.MapGet("/GetActiveUserList", async () => await userManager.GetActiveUserList());
            app.MapGet("/GetActiveUsersListsDividedBy100", async () => await userManager.GetActiveUserCountDividedBy100());
            app.MapGet("/GetActiveUserList/s={itemsToSkip}", async (int itemsToSkip) => await userManager.GetActiveUserList(itemsToSkip));

            return app;
        }

        public static WebApplication MapAdminEndpoints(this WebApplication app, IAdminManager adminManager)
        {
            app.MapPost("/admin/UpdateRole", async (RoleDto roleDto) => await adminManager.UpdateRole(roleDto));
            app.MapDelete("/admin/DeleteRole/{roleId}", async (string roleId) => await adminManager.DeleteRole(roleId));
            app.MapPost("/admin/AddRole", async (string roleName) => await adminManager.AddNewRole(roleName));
            app.MapGet("/admin/GetRoles", async () => await adminManager.GetRoles());
            app.MapGet("/admin/GetRoles/{roleId}", async (string roleId) => await adminManager.GetRoleById(roleId));
            app.MapPost("/admin/RegisterNewUser", async (UserAdminRegistrationDto userDto) => await adminManager.RegisterNewUser(userDto));
            app.MapPost("/admin/UpdateUser", async (UserAdminAccountUpdateDto userDto) => await adminManager.UpdateUserAccount(userDto));
            app.MapDelete("/admin/DeleteUser/{userId}", async (string userId) => await adminManager.DeleteUser(userId));
            app.MapPost("/admin/ChangePassword", async (UserAdminPasswordChangeDto passwordChangeDto) => await adminManager.ChangeUserPassword(passwordChangeDto));
            app.MapGet("/admin/GetAllUsers", async () => await adminManager.GetAllUsers());
            app.MapGet("/admin/GetUser", async (string userId) => await adminManager.GetUser(userId));
            return app;
        }
    }
}
