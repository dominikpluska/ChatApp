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
            app.MapGet("/GetAllChats", async (CancellationToken cancellationToken) => await chatsManager.GetAllChats(cancellationToken));
            app.MapPost("/CreateChatsTable/{userId}", async (string userId, CancellationToken cancellationToken) => await chatsManager.CreateChatsTable(userId, cancellationToken)).AllowAnonymous();
            return app;
        }

        public static WebApplication MapBlackListsEndpoints(this WebApplication app, IBlackListsManager blackListsManager)
        {
            app.MapGet("/GetBlackList", async (CancellationToken cancellationToken) => await blackListsManager.GetBlackList(cancellationToken));
            app.MapGet("/GetUserFromBlackList/u={userId}/c={chatterId}", async (string userId, string chatterId, CancellationToken cancellationToken) => await blackListsManager.GetUserFromBlackList(userId, chatterId, cancellationToken));
            app.MapPost("/CreateBlackListTable/{userId}", async (string userId, CancellationToken cancellationToken) => await blackListsManager.CreateBlackListTable(userId, cancellationToken)).AllowAnonymous();
            app.MapPost("/AddToBlackList/{blockedId}", async (string blockedId, CancellationToken cancellationToken) => await blackListsManager.AddUserToBlackList(blockedId, cancellationToken));
            app.MapDelete("/RemoveFromBlackList/{blockedId}", async (string blockedId, CancellationToken cancellationToken) => await blackListsManager.RemoveFromBlackList(blockedId, cancellationToken));
            return app;
        }

        public static WebApplication MapFriendsManagerEndpoints(this WebApplication app, IFriendManager friendManager)
        {
            app.MapGet("/GetAllFriends", async (CancellationToken cancellationToken) => await friendManager.GetFriendsList(cancellationToken));
            app.MapPost("/SentFriendRequest/{userId}", async (string userId, CancellationToken cancellationToke) => await friendManager.SendFriendRequests(userId, cancellationToke));
            app.MapPost("/CreateFriendsListTable/{userId}", async (string userId, CancellationToken cancellationToke) => await friendManager.CreateFriendsListTable(userId, cancellationToke)).AllowAnonymous();
            app.MapPut("/AcceptFriendRequest/{requestId}", async (string requestId, CancellationToken cancellationToke) => await friendManager.AcceptFriendRequest(requestId, cancellationToke));
            app.MapDelete("/RemoveFriend/{friendId}", async (string friendId, CancellationToken cancellationToke) => await friendManager.RemoveFriend(friendId, cancellationToke));
            app.MapDelete("/RejectFriendRequest/{requestId}", async (string requestId, CancellationToken cancellationToke) => await friendManager.RejectFriendRequest(requestId, cancellationToke));

            return app;
        }

        public static WebApplication MapRequestsManagerEndpoints(this WebApplication app, IRequestManager requestManager)
        {
            app.MapGet("/GetAllRequests", async (CancellationToken cancellationToken) => await requestManager.GetAllRequests(cancellationToken));
            return app;
        }

        public static WebApplication MapHubs(this WebApplication app)
        {
            app.MapHub<UserSettingsHub.UserSettingsHub>("/UserSettings");
            return app;
        }
    }
}
