using MessagesApi.Dto;
using MessagesApi.Models;
using MongoDB.Bson;

namespace MessagesApi.DatabaseOperations.Commands.ChatCommands
{
    public interface IChatCommands
    {
        public Task<IResult> Insert(Chat chat);
        public Task<IResult> InsertNewMessage(ObjectId chatId, Message message);
        public Task<IResult> AcceptChatRequest(ObjectId chatId, string userId);
        public Task<IResult> UpdateMessage(MessageUpdateDto messageUpdateDto);
        public Task<IResult> DropChat(ObjectId chatId);
        public Task<string> CreateChat(Chat chat);
        public Task<IResult> DeleteMessage(ObjectId chatId, ObjectId messageId);
        public Task<IResult> RemoveChatParticipant(ObjectId chatId, string userId);

    }
}
