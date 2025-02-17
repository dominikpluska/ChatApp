using MongoDB.Bson;
using UserSettingsApi.Models;

namespace UserSettingsApi.DatabaseOperations.Repository.FriendRequestsRepository
{
    public interface IRequestsRepository
    {
        public Task<Request> GetRequest(ObjectId requestId);
        public Task<Request> GetRequest(string requestorId, string requesteeId);
        public Task<IEnumerable<Request>> GetAllRequests(string userId);
    }
}
