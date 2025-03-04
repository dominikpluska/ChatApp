using MessagesApi.Dto;
using MessagesApi.Models;
using MongoDB.Bson;

namespace MessagesApi.DatabaseOperations.Repository.ChatRepository
{
    public interface IChatRepository
    {
        public Task<IEnumerable<Message>> GetChatMessages(ObjectId chatId, CancellationToken cancellationToken);
        public Task<ObjectId> CheckChat(ObjectId chatId, CancellationToken cancellationToken);
        public Task<string> FindChat(string userId, string chatterId, CancellationToken cancellationToken);
        public Task<Message> GetChatMessage(ObjectId chatId, ObjectId messageId, CancellationToken cancellationToken);
        public Task<IEnumerable<string>> GetChatParticipants(ObjectId chatId, CancellationToken cancellationToken);
    }
}
