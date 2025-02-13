using UserSettingsApi.Dto;
using UserSettingsApi.Managers.BlackListsManager;
using UserSettingsApi.Managers.ChatsManager;
using UserSettingsApi.Managers.FriendsListsManager;

namespace UserSettingsApi
{
    public static class MapEndpoints
    {
        public static WebApplication MapChatsEndpoints(this WebApplication app, IChatsManager chatsManager)
        {
            app.MapGet("/GetAllChats", async () => await chatsManager.GetAllChats());
            app.MapPost("/CreateChatsTable/{userId}", async (string userId) => await chatsManager.CreateChatsTable(userId));
            return app;
        }

        public static WebApplication MapBlackListsEndpoints(this WebApplication app, IBlackListsManager blackListsManager)
        {
            app.MapGet("/GetBlackList", async () => await blackListsManager.GetBlackList());
            app.MapPost("/CreateBlackListTable/{userId}", async (string userId) => await blackListsManager.CreateBlackListTable(userId));
            return app;
        }

        public static WebApplication MapFriendsManagerEndpoints(this WebApplication app, IFriendManager friendManager)
        {
            app.MapGet("/GetAllFriends", async () => await friendManager.GetFriendsList());
            app.MapPost("/SentFriendRequest/{userId}", async (string userId) => await friendManager.SendFriendRequests(userId));
            app.MapPost("/CreateFriendsListTable/{userId}", async (string userId) => await friendManager.CreateFriendsListTable(userId));
            app.MapPut("/AcceptFriendRequest/{requestId}", async (string requestId) => await friendManager.AcceptFriendRequest(requestId));
            return app;
        }
    }
}
