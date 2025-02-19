using MessagesApi.Dto;
using MessagesApi.Managers.ChatManager;
using MessagesApi.MessagesHub;

namespace MessagesApi
{
    public static class MapEndpoints
    {
        public static WebApplication MapMessagesEndpoints(this WebApplication app, IChatManager chatManager)
        {
            app.MapPost("/PostMessage", async (MessageDto messageDto) => await chatManager.PostMessage(messageDto));
            app.MapPut("/UpdateMessage", async (MessageUpdateDto messageUpdateDto) => await chatManager.UpdateChatMessage(messageUpdateDto));
            app.MapGet("GetChatMessages/{chatId}", async (string chatId) => await chatManager.GetMessages(chatId));
            app.MapGet("/GetChatMessage/c={chatId}/m={messageId}", async (string chatId, string messageId) => await chatManager.GetMessage(chatId, messageId));
            app.MapGet("/OpenChat/{chatterId}", async (string chatterId) => await chatManager.OpenChat(chatterId));
            app.MapDelete("/DeleteChatMessage/c={chatId}/m={messageId}", async (string chatId, string messageId) => await chatManager.DeleteChatMessage(chatId, messageId));
            app.MapDelete("/LeaveChat/{chatId}", async (string chatId) => await chatManager.LeaveChat(chatId));
            return app;
        }

        public static WebApplication MapHubs(this WebApplication app)
        {
            app.MapHub<MessagesHub.MessagesHub>("/Chat");
            return app;
        }
    }
}
