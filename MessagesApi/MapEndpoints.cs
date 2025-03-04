using MessagesApi.Dto;
using MessagesApi.Managers.ChatManager;
using MessagesApi.MessagesHub;

namespace MessagesApi
{
    public static class MapEndpoints
    {
        public static WebApplication MapMessagesEndpoints(this WebApplication app, IChatManager chatManager)
        {
            app.MapPost("/PostMessage", async (MessageDto messageDto, CancellationToken cancellationToken) => await chatManager.PostMessage(messageDto, cancellationToken));
            app.MapPut("/UpdateMessage", async (MessageUpdateDto messageUpdateDto, CancellationToken cancellationToken) => await chatManager.UpdateChatMessage(messageUpdateDto, cancellationToken));
            app.MapGet("GetChatMessages/{chatId}", async (string chatId, CancellationToken cancellationToken) => await chatManager.GetMessages(chatId, cancellationToken));
            app.MapGet("/GetChatMessage/c={chatId}/m={messageId}", async (string chatId, string messageId, CancellationToken cancellationToken) => await chatManager.GetMessage(chatId, messageId, cancellationToken));
            app.MapGet("/OpenChat/{chatterId}", async (string chatterId, CancellationToken cancellationToken) => await chatManager.OpenChat(chatterId, cancellationToken));
            app.MapDelete("/DeleteChatMessage/c={chatId}/m={messageId}", async (string chatId, string messageId, CancellationToken cancellationToken) => await chatManager.DeleteChatMessage(chatId, messageId, cancellationToken));
            app.MapDelete("/LeaveChat/{chatId}", async (string chatId, CancellationToken cancellationToken) => await chatManager.LeaveChat(chatId, cancellationToken));
            return app;
        }

        public static WebApplication MapHubs(this WebApplication app)
        {
            app.MapHub<MessagesHub.MessagesHub>("/Chat");
            return app;
        }
    }
}
