using MongoDB.Bson;
using MongoDB.Driver;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.FriendsLisiCommands
{
    public class FriendListCommands : IFriendListCommands
    {
        private readonly MongoDBService _mongoDbService;

        public FriendListCommands(MongoDBService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<IResult> CreateFriendList(FriendsList friendsList)
        {
            ArgumentNullException.ThrowIfNull(friendsList);
            await _mongoDbService.FriendsListCollection.InsertOneAsync(friendsList);
            return Results.Ok("Friends has been created");
        }

        public async Task<IResult> AddNewFriend(ObjectId friendsListId , string friendId)
        {
            ArgumentNullException.ThrowIfNull(friendId);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", friendsListId);
            var insert = Builders<BsonDocument>.Update.Push("Friends", friendId);

            var result = await _mongoDbService.FriendsListCollectionBson.FindOneAndUpdateAsync(filter, insert);
            return Results.Ok(result);
        }

        public async Task<IResult> RemoveFriend(ObjectId friendsListId, string friendId)
        {

            ArgumentNullException.ThrowIfNull(friendId);

            var filter = Builders<BsonDocument>.Filter.Eq("_id", friendsListId);
            var delete = Builders<BsonDocument>.Update.Pull("Friends", friendId);

            var result = await _mongoDbService.FriendsListCollectionBson.UpdateOneAsync(filter, delete);
            return Results.Ok(result);

        }
    }
}
