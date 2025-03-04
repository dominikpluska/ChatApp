using AuthApi.Dto;
using AuthApi.Managers.AdminManager;
using AuthApi.Managers.UserManager;
using AuthApi.UserAccessor;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.CompilerServices;

namespace AuthApi
{
    public static class MapEndpoints
    {
        public static WebApplication MapUserEndpoints(this WebApplication app, IUserManager userManager)
        {
            using var scope = app.Services.CreateScope();
            var userAccessor = scope.ServiceProvider.GetRequiredService<IUserAccessor>();

            app.MapPost("/Register", async  (UserRegistrationDto userDto, CancellationToken cancellationToken) => await userManager.RegisterNewUser(userDto, cancellationToken)).AllowAnonymous();
            app.MapPost("/Login", async (UserLoginDto loginDto, CancellationToken cancellationToken) => await userManager.Login(loginDto, cancellationToken)).AllowAnonymous();
            app.MapGet("/AuthCheck", async (CancellationToken cancellationToken) => await userManager.CheckAuthentication(cancellationToken)).AllowAnonymous();
            app.MapPost("/Logout", async ( CancellationToken cancellationToken) => await userManager.LogOut(cancellationToken));
            app.MapPost("/ChangePassword", async (NewPasswordDto passwordChange, CancellationToken cancellationToken) => await userManager.UpdatePassword(passwordChange, cancellationToken));
            app.MapPost("/UpdateUser", async (UserAccountUpdateDto userDto, CancellationToken cancellationToken) => await userManager.UpdateAccount(userDto, cancellationToken));
            app.MapGet("/GetAccountProperties", async (string userId, CancellationToken cancellationToken) => await userManager.GetAccountProperties(userId, cancellationToken));
            app.MapGet("/GetActiveUserList", async (CancellationToken cancellationToken) => await userManager.GetActiveUserList(cancellationToken));
            app.MapGet("/GetActiveUserList/s={itemsToSkip}", async (int itemsToSkip, CancellationToken cancellationToken) => await userManager.GetActiveUserList(cancellationToken, itemsToSkip));
            app.MapGet("/Search/{userName}", async (string userName, CancellationToken cancellationToken) => await userManager.SearchForUser(userName, cancellationToken));
            app.MapPost("/GetUserListByIds", async (IdRequestsDto IdRequests, CancellationToken cancellationToken) => await userManager.PostUserListByIds(IdRequests.Ids, cancellationToken));

            return app;
        }

        public static WebApplication MapAdminEndpoints(this WebApplication app, IAdminManager adminManager)
        {
            app.MapPost("/admin/UpdateRole", async (RoleDto roleDto, CancellationToken cancellation) => await adminManager.UpdateRole(roleDto, cancellation));
            app.MapDelete("/admin/DeleteRole/{roleId}", async (string roleId, CancellationToken cancellation) => await adminManager.DeleteRole(roleId, cancellation));
            app.MapPost("/admin/AddRole", async (string roleName, CancellationToken cancellation) => await adminManager.AddNewRole(roleName, cancellation));
            app.MapGet("/admin/GetRoles", async (CancellationToken cancellation) => await adminManager.GetRoles(cancellation));
            app.MapGet("/admin/GetRoles/{roleId}", async (string roleId, CancellationToken cancellation) => await adminManager.GetRoleById(roleId, cancellation));
            app.MapPost("/admin/RegisterNewUser", async (UserAdminRegistrationDto userDto, CancellationToken cancellation) => await adminManager.RegisterNewUser(userDto, cancellation));
            app.MapPost("/admin/UpdateUser", async (UserAdminAccountUpdateDto userDto, CancellationToken cancellation) => await adminManager.UpdateUserAccount(userDto, cancellation));
            app.MapDelete("/admin/DeleteUser/{userId}", async (string userId, CancellationToken cancellation) => await adminManager.DeleteUser(userId, cancellation));
            app.MapPost("/admin/ChangePassword", async (UserAdminPasswordChangeDto passwordChangeDto, CancellationToken cancellation) => await adminManager.ChangeUserPassword(passwordChangeDto, cancellation));
            app.MapGet("/admin/GetAllUsers", async (CancellationToken cancellation) => await adminManager.GetAllUsers(cancellation));
            app.MapGet("/admin/GetUser", async (string userId, CancellationToken cancellation) => await adminManager.GetUser(userId, cancellation));
            return app;
        }
    }
}
