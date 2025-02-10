using UserSettingsApi.Dto;
using UserSettingsApi.Managers.ChatsManager;

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
    }
}
