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
            app.MapPost("/CreateChatsTable", async (UserSettingsDto userSettingsDto) => await chatsManager.CreateChatsTable(userSettingsDto));
            return app;
        }

        public static WebApplication MapBlackListsEndpoints(this WebApplication app, IBlackListsManager blackListsManager)
        {
            app.MapPost("/CreateBlackListTable", async (UserSettingsDto userSettingsDto) => await blackListsManager.CreateBlackListTable(userSettingsDto));
            return app;
        }

        public static WebApplication MapFriendsListEndpoints(this WebApplication app, IFriendsListsManager friendsListsManager)
        {
            app.MapGet("/GetAllFriends", async () => await friendsListsManager.GetFriendsList());
            app.MapPost("/CreateFriendsListTable", async (UserSettingsDto userSettingsDto) => await friendsListsManager.CreateFriendsListTable(userSettingsDto));
            app.MapPut("/AddNewFriend", async (UserSettingsDto userSettingsDto) => await friendsListsManager.AddNewFriend(userSettingsDto));
            return app;
        }
    }
}
