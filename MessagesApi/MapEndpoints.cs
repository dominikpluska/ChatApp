﻿using MessagesApi.Dto;
using MessagesApi.Managers.ChatManager;

namespace MessagesApi
{
    public static class MapEndpoints
    {
        public static WebApplication MapMessagesEndpoints(this WebApplication app, IChatManager chatManager)
        {
            app.MapPost("/PostMessage", async (MessageDto messageDto) => await chatManager.PostMessage(messageDto));
            app.MapPost("/RequestChat", async (ChatRequestDto chatRequestDto) => await chatManager.SendChatRequest(chatRequestDto));
            app.MapPost("/AcceptChat", async (AcceptChatRequestDto acceptChatRequestDto) => await chatManager.AcceptChatRequest(acceptChatRequestDto));
            app.MapPut("/UpdateMessage", async (MessageUpdateDto messageUpdateDto) => await chatManager.UpdateChatMessage(messageUpdateDto));
            app.MapGet("GetChatMessages/{chatId}", async (string chatId) => await chatManager.GetMessages(chatId));
            app.MapGet("/GetChatMessage/c={chatId}/m={messageId}", async (string chatId, string messageId) => await chatManager.GetMessage(chatId, messageId));
            app.MapDelete("/DeleteChatMessage/c={chatId}/m={messageId}", async (string chatId, string messageId) => await chatManager.DeleteChatMessage(chatId, messageId));
            app.MapDelete("/LeaveChat/{chatId}", async (string chatId) => await chatManager.LeaveChat(chatId));
            return app;
        }
    }
}
