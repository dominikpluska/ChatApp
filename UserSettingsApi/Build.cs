using UserSettingsApi.Data;
using UserSettingsApi.DatabaseOperations.Commands.BlackListCommands;
using UserSettingsApi.DatabaseOperations.Commands.ChatsCommands;
using UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands;
using UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands;
using UserSettingsApi.DatabaseOperations.Repository.BlackListRepository;
using UserSettingsApi.DatabaseOperations.Repository.ChatsRepository;
using UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository;
using UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository;
using UserSettingsApi.Managers.BlackListsManager;
using UserSettingsApi.Managers.ChatsManager;
using UserSettingsApi.Managers.FriendsListsManager;
using UserSettingsApi.Services;
using UserSettingsApi.UserAccessor;


namespace UserSettingsApi
{
    public static class Build
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<MongoDBService>();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<IChatsManager, ChatsManager>();
            builder.Services.AddScoped<IFriendManager, FriendManager>();
            builder.Services.AddScoped<IBlackListsManager, BlackListsManager>();

            builder.Services.AddScoped<IChatsRepository, ChatsRepository>();
            builder.Services.AddScoped<IFriendsListRepository, FriendsListRepository>();
            builder.Services.AddScoped<IFriendRequestsRepository, FriendRequestsRepository>();
            builder.Services.AddScoped<IBlackListRepository, BlackListRepository>();

            builder.Services.AddScoped<IChatsCommands, ChatsCommands>();
            builder.Services.AddScoped<IFriendListCommands, FriendListCommands>();
            builder.Services.AddScoped<IBlackListCommands, BlackListCommands>();
            builder.Services.AddScoped<IFriendRequestCommands, FriendRequestCommands>();
            
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserAccessor, HttpUserAccessor>();

            return builder;
        }
    }
}
