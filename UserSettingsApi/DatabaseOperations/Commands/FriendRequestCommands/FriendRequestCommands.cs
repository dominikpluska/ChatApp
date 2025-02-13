using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.AccessControl;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands
{
    public class FriendRequestCommands : IFriendRequestCommands
    {
        private readonly MongoDBService _mongoDBService;

        public FriendRequestCommands(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IResult> InsertFriendRequests(FriendRequest friendRequest)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(friendRequest);
                await _mongoDBService.FriendRequestsCollection.InsertOneAsync(friendRequest);
                return Results.Ok("Friend request has been sent");
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> DeleteFriendRequest(ObjectId friendRequestId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(friendRequestId);

                var filter = Builders<FriendRequest>.Filter.Eq(x => x.RequestId, friendRequestId);
                await _mongoDBService.FriendRequestsCollection.DeleteOneAsync(filter);

                return Results.Ok("Friend request has been sent");
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IResult> AcceptFriendRequest(ObjectId friendRequestId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(friendRequestId);

                var filter = Builders<FriendRequest>.Filter.Eq(x => x.RequestId, friendRequestId);
                var update = Builders<FriendRequest>.Update.Set(x => x.IsAccepted, true);

                await _mongoDBService.FriendRequestsCollection.UpdateOneAsync(filter, update);

                return Results.Ok("Chat updated!");

            }
            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
