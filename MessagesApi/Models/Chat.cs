using MessagesApi.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace MessagesApi.Models
{
    [Collection("ChatTable")]
    public class Chat
    {
        [BsonId]
        public ObjectId ChatId { get; set; } = ObjectId.GenerateNewId();
        public required List<ChatParticipant> ChatParticipants { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();

    }
}
