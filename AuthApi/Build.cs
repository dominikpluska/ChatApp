using AuthApi.Cookies;
using AuthApi.DataBaseOperations.Commands.RoleCommands;
using AuthApi.DataBaseOperations.Commands.UserAccountsCommands;
using AuthApi.DataBaseOperations.Repository.RoleRepository;
using AuthApi.DataBaseOperations.Repository.UserAccountsRepository;
using AuthApi.Jwt;
using AuthApi.Managers.AdminManager;
using AuthApi.Managers.UserManager;
using AuthApi.Services;
using AuthApi.UserAccessor;

namespace AuthApi
{
    public static class Build
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IUserAccountsRepository, UserAccountsRepository>();
            builder.Services.AddScoped<IUserAccountsCommands, UserAccountsCommands>();

            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IRoleCommands, RoleCommands>();


            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IAdminManager, AdminManager>();
            builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();

            builder.Services.AddScoped<ICookieGenerator, CookieGenerator>();
            builder.Services.AddScoped<IUserAccessor, HttpUserAccessor>();
            builder.Services.AddScoped<IJwt, AuthApi.Jwt.Jwt>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return builder;
        }
    }
}
