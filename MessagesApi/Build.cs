using MessagesApi.Data;
using MessagesApi.DatabaseOperations.Commands.ChatCommands;
using MessagesApi.DatabaseOperations.Repository.ChatRepository;
using MessagesApi.Managers.ChatManager;
using MessagesApi.Services;
using MessagesApi.UserAccessor;

namespace MessagesApi
{
    public static class Build
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddAuthorization();
            builder.Services.AddSignalR();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IChatCommands, ChatCommands>();
            builder.Services.AddScoped<IChatRepository, ChatRepository>();
            builder.Services.AddScoped<IUserAccessor, HttpUserAccessor>();
            builder.Services.AddScoped<IChatManager, ChatManager>();
            builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<MongoDBService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return builder;
        }
    }
}
