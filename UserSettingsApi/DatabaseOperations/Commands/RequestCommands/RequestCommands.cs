using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.AccessControl;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Commands.FriendRequestCommands
{
    public class RequestCommands : IRequestCommands
    {
        private readonly MongoDBService _mongoDBService;

        public RequestCommands(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<IResult> InsertRequests(Request request)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(request);
                await _mongoDBService.RequestsCollection.InsertOneAsync(request);
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

        public async Task<IResult> DeleteRequest(ObjectId requestId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(requestId);

                var filter = Builders<Request>.Filter.Eq(x => x.RequestId, requestId);
                await _mongoDBService.RequestsCollection.DeleteOneAsync(filter);

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

        public async Task<IResult> AcceptRequest(ObjectId requestId)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(requestId);

                var filter = Builders<Request>.Filter.Eq(x => x.RequestId, requestId);
                var update = Builders<Request>.Update.Set(x => x.IsAccepted, true);

                await _mongoDBService.RequestsCollection.UpdateOneAsync(filter, update);

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
