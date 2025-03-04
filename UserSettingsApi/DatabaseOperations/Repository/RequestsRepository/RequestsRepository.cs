using MongoDB.Bson;
using MongoDB.Driver;
using UserSettingsApi.Data;
using UserSettingsApi.DatabaseOperations.Repository.RequestsRepository;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.RequestsRepository
{
    public class RequestsRepository : IRequestsRepository
    {
        private readonly MongoDBService _mongoDBService;

        public RequestsRepository(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        public async Task<Request> GetRequest(ObjectId requestId, CancellationToken cancellationToken)
        {

            var filter = Builders<Request>.Filter.Eq(x => x.RequestId, requestId);

            var result = await _mongoDBService.RequestsCollection.FindAsync(filter, cancellationToken: cancellationToken);

            return await result.FirstOrDefaultAsync();

        }

        public async Task<Request> GetRequest(string requestorId, string requesteeId, CancellationToken cancellationToken)
        {

            var filter = Builders<Request>.Filter.Eq(x => x.RequestorId, requestorId) &
                         Builders<Request>.Filter.Eq(x => x.RequesteeId, requesteeId);

            var result = await _mongoDBService.RequestsCollection.FindAsync(filter, cancellationToken: cancellationToken);

            return await result.FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Request>> GetAllRequests(string userId, CancellationToken cancellationToken)
        {

            var filter = Builders<Request>.Filter.Eq(x => x.RequesteeId, userId);

            var result = await _mongoDBService.RequestsCollection.FindAsync(filter, cancellationToken: cancellationToken);

            return await result.ToListAsync();

        }

    }
}
