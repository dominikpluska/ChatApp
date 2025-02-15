using MongoDB.Bson;
using MongoDB.Driver;
using UserSettingsApi.Data;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository
{
    public class RequestsRepository : IRequestsRepository
    {
        private readonly MongoDBService _mongoDBService;

        public RequestsRepository(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<Request> GetRequests(ObjectId requestId)
        {
            try
            {
                var filter = Builders<Request>.Filter.Eq(x => x.RequestId, requestId);

                var result = await _mongoDBService.RequestsCollection.FindAsync(filter);

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

        public async Task<IEnumerable<Request>> GetAllRequests(string userId)
        {
            try
            {
                var filter = Builders<Request>.Filter.Eq(x => x.RequesteeId, userId);

                var result = await _mongoDBService.RequestsCollection.FindAsync(filter);

                return await result.ToListAsync();
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Argument null exception", ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
