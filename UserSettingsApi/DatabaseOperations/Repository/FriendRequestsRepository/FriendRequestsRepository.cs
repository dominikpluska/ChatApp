using MongoDB.Bson;
using MongoDB.Driver;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository
{
    public class FriendRequestsRepository : IFriendRequestsRepository
    {
        private readonly MongoDBService _mongoDBService;

        public FriendRequestsRepository(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<FriendRequest> GetFriendRequest(ObjectId friendRequestId)
        {
            try
            {
                var filter = Builders<FriendRequest>.Filter.Eq(x => x.RequestId, friendRequestId);

                var result = await _mongoDBService.FriendRequestsCollection.FindAsync(filter);

                return await result.FirstOrDefaultAsync();
            }
            catch(ArgumentNullException ex)
            {
                throw new ArgumentNullException("Argument null exception", ex.Message);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
