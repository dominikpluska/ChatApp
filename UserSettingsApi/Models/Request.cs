using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserSettingsApi.Models
{
    public class Request
    {
        [BsonId]
        public ObjectId RequestId { get; set; } = ObjectId.GenerateNewId();
        public required string RequestorId { get; set; }
        public required string RequesteeId { get; set; }
        public string RequestType { get; set; } = "FriendRequest";
        public bool IsAccepted { get; set; } = false;
    }
}
