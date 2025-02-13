using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserSettingsApi.Models
{
    public class Chat
    {
        [BsonId]
        public ObjectId ChatsId { get; set; }
        public required string UserId { get; set; }
        public List<string>? ChatList { get; set; } = new List<string>();
    }
}
