using MessagesApi.Dto;
using MessagesApi.Models;
using MongoDB.Bson;

namespace MessagesApi.DatabaseOperations.Repository.ChatRepository
{
    public interface IChatRepository
    {
        public Task<IEnumerable<Message>> GetChatMessages(ObjectId chatId);
        public Task<ObjectId> CheckChat(ObjectId chatId);
        public Task<string> FindChat(string userId, string chatterId);
        public Task<Message> GetChatMessage(ObjectId chatId, ObjectId messageId);
        public Task<IEnumerable<string>> GetChatParticipants(ObjectId chatId);
    }
}
