using MessagesApi.Dto;
using Microsoft.AspNetCore.SignalR;

namespace MessagesApi.MessagesHub
{
    public class MessagesHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected to the chat session!");
        }

        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task LeaveChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task PostMessage(string chatId, MessageRetrivedDto messageReceived)
        {
            await Clients.Group(chatId).SendAsync("ReceiveMessage", messageReceived);
        }
    }
}
