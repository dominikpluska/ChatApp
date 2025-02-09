using MessagesApi.Dto;
using MessagesApi.Models;
using MongoDB.Bson;

namespace MessagesApi.DatabaseOperations.Repository.ChatRepository
{
    public interface IChatRepository
    {
        public Task<IEnumerable<Message>> GetChatMessages(ObjectId chatId);
        public Task<ObjectId> CheckChat(ObjectId chatId);
        public Task<Message> GetChatMessage(ObjectId chatId, ObjectId messageId);
        public Task<IEnumerable<ChatParticipant>> GetChatParticipants(ObjectId chatId);
    }
}
