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
            try
            {
                var filter = Builders<FriendsList>.Filter.Eq(x => x.UserAccountId, userId);
                var chatsList = await _mongoDBService.FriendsListCollection.Find(filter).FirstOrDefaultAsync();
                return chatsList;
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ObjectId> GetFriendsListId(string userId)
        {
            try
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
            catch (ArgumentNullException ex)
            {
                throw new ArgumentException("Argument Null Exception!", ex.Message);
            }
            catch (OperationCanceledException ex)
            {
                throw new OperationCanceledException($"Operation Canceled Exception! {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
