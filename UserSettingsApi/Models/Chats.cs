using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserSettingsApi.Models
{
    public class Chats
    {
        [BsonId]
        public ObjectId LastsChatsId { get; set; }
        public required string UserId { get; set; }
        public required List<string> ChatList { get; set; }
    }
}
