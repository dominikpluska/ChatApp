using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserSettingsApi.Models
{
    public class FriendRequest
    {
        [BsonId]
        public ObjectId RequestId { get; set; }
        public required string RequestorId { get; set; }
        public required string RequesteeId { get; set; }
        public bool IsAccepted { get; set; } = false;
    }
}
