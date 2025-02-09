using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessagesApi.Models
{
    public class Message
    {
        [BsonId]
        public ObjectId MessageId { get; set; } = ObjectId.GenerateNewId();
        public required string UserId { get; set; }
        public required string TextMessage { get; set; }
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    }
}
