using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserSettingsApi.Models
{
    public class FriendsList
    {
        [BsonId]
        public ObjectId FriendsListId { get; set; }
        public required string UserAccountId { get; set; }
        public required List<Friend> Friends { get; set; }
    }
}
