using UserSettingsApi.Dto;
using UserSettingsApi.Managers.BlackListsManager;
using UserSettingsApi.Managers.ChatsManager;
using UserSettingsApi.Managers.FriendsListsManager;
using UserSettingsApi.Managers.RequestsManager;
using UserSettingsApi.UserSettingsHub;

namespace UserSettingsApi
{
    public static class MapEndpoints
    {
        public static WebApplication MapChatsEndpoints(this WebApplication app, IChatsManager chatsManager)
        {
            app.MapGet("/GetAllChats", async () => await chatsManager.GetAllChats());
            app.MapPost("/CreateChatsTable/{userId}", async (string userId) => await chatsManager.CreateChatsTable(userId)).AllowAnonymous();
            return app;
        }

        public static WebApplication MapBlackListsEndpoints(this WebApplication app, IBlackListsManager blackListsManager)
        {
            app.MapGet("/GetBlackList", async () => await blackListsManager.GetBlackList());
            app.MapGet("/GetUserFromBlackList/u={userId}/c={chatterId}", async (string userId, string chatterId) => await blackListsManager.GetUserFromBlackList(userId, chatterId));
            app.MapPost("/CreateBlackListTable/{userId}", async (string userId) => await blackListsManager.CreateBlackListTable(userId)).AllowAnonymous();
            app.MapPost("/AddToBlackList/{blockedId}", async (string blockedId) => await blackListsManager.AddUserToBlackList(blockedId));
            app.MapDelete("/RemoveFromBlackList/{blockedId}", async (string blockedId) => await blackListsManager.RemoveFromBlackList(blockedId));
            return app;
        }

        public static WebApplication MapFriendsManagerEndpoints(this WebApplication app, IFriendManager friendManager)
        {
            app.MapGet("/GetAllFriends", async () => await friendManager.GetFriendsList());
            app.MapPost("/SentFriendRequest/{userId}", async (string userId) => await friendManager.SendFriendRequests(userId));
            app.MapPost("/CreateFriendsListTable/{userId}", async (string userId) => await friendManager.CreateFriendsListTable(userId)).AllowAnonymous();
            app.MapPut("/AcceptFriendRequest/{requestId}", async (string requestId) => await friendManager.AcceptFriendRequest(requestId));
            app.MapDelete("/RemoveFriend/{friendId}", async (string friendId) => await friendManager.RemoveFriend(friendId));
            app.MapDelete("/RejectFriendRequest/{requestId}", async (string requestId) => await friendManager.RejectFriendRequest(requestId));

            return app;
        }

        public static WebApplication MapRequestsManagerEndpoints(this WebApplication app, IRequestManager requestManager)
        {
            app.MapGet("/GetAllRequests", async () => await requestManager.GetAllRequests());
            return app;
        }

        public static WebApplication MapHubs(this WebApplication app)
        {
            app.MapHub<UserSettingsHub.UserSettingsHub>("/UserSettings");
            return app;
        }
    }
}
