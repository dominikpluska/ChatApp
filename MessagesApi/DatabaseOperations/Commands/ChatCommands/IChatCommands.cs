using MessagesApi.Dto;
using MessagesApi.Models;
using MongoDB.Bson;

namespace MessagesApi.DatabaseOperations.Commands.ChatCommands
{
    public interface IChatCommands
    {
        public Task<IResult> Insert(Chat chat, CancellationToken cancellationToken);
        public Task<IResult> InsertNewMessage(ObjectId chatId, Message message, CancellationToken cancellationToken);
        public Task<IResult> AcceptChatRequest(ObjectId chatId, string userId, CancellationToken cancellationToken);
        public Task<IResult> UpdateMessage(MessageUpdateDto messageUpdateDto, CancellationToken cancellationToken);
        public Task<IResult> DropChat(ObjectId chatId, CancellationToken cancellationToken);
        public Task<string> CreateChat(Chat chat, CancellationToken cancellationToken);
        public Task<IResult> DeleteMessage(ObjectId chatId, ObjectId messageId, CancellationToken cancellationToken);
        public Task<IResult> RemoveChatParticipant(ObjectId chatId, string userId, CancellationToken cancellationToken);

    }
}
