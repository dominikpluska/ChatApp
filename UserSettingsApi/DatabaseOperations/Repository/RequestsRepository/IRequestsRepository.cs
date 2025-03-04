using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.RequestsRepository
{
    public interface IRequestsRepository
    {
        public Task<Request> GetRequest(ObjectId requestId, CancellationToken cancellationToken);
        public Task<Request> GetRequest(string requestorId, string requesteeId, CancellationToken cancellationToken);
        public Task<IEnumerable<Request>> GetAllRequests(string userId, CancellationToken cancellationToken);
    }
}
