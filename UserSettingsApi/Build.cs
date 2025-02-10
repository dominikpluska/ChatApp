using Microsoft.AspNetCore.Authentication;
using UserSettingsApi.Data;
using UserSettingsApi.DatabaseOperations.Commands.ChatsCommands;
using UserSettingsApi.DatabaseOperations.Repository.ChatsRepository;
using UserSettingsApi.Managers.ChatsManager;
using UserSettingsApi.UserAccessor;

namespace UserSettingsApi
{
    public static class Build
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<MongoDBService>();
            builder.Services.AddScoped<IChatsManager, ChatsManager>();
            builder.Services.AddScoped<IChatsRepository, ChatsRepository>();
            builder.Services.AddScoped<IChatsCommands, ChatsCommands>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IUserAccessor, HttpUserAccessor>();
            builder.Services.AddHttpClient();
            return builder;
        }
    }
}
