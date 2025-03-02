using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendsListRepository
{
    public class FriendsListRepository : IFriendsListRepository
    {
        private readonly MongoDBService _mongoDBService;

        public FriendsListRepository(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<FriendsList> GetFriendsList(string userId)
        {

            var filter = Builders<FriendsList>.Filter.Eq(x => x.UserAccountId, userId);
            var chatsList = await _mongoDBService.FriendsListCollection.Find(filter).FirstOrDefaultAsync();
            return chatsList;
        }

        public async Task<ObjectId> GetFriendsListId(string userId)
        {

            var filter = Builders<FriendsList>.Filter.Eq(x => x.UserAccountId, userId);
            var projection = Builders<FriendsList>.Projection
                              .Include(x => x.FriendsListId)
                              .Exclude(x => x.Friends)
                              .Exclude(x => x.UserAccountId);

            var result = await _mongoDBService.FriendsListCollection.Find(filter).Project(projection).FirstOrDefaultAsync();

            var selectedId = result.GetValue("_id");
            var deserializedResult = BsonSerializer.Deserialize<ObjectId>(selectedId.ToJson());

            return deserializedResult;

        }
    }
}
